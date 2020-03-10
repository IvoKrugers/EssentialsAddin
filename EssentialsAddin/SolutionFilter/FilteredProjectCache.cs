using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Cairo;
using EssentialsAddin.Helpers;
using Gdk;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Pads.ProjectPad;
using MonoDevelop.Projects;

namespace EssentialsAddin.SolutionFilter
{
    public static class FilteredProjectCache
    {
        struct TreeItem
        {
            public FilePath ProjectVirtualPath;
            public bool IsExpanded;
        }

        private static Dictionary<string, DateTime> _projectsDictionary = new Dictionary<string, DateTime>();
        //private Dictionary<string, List<string>> _folderDictionary = new Dictionary<string, List<string>>();
        private static Dictionary<string, TreeItem> _treeDictionary = new Dictionary<string, TreeItem>();

        private static string _lastFilter = string.Empty;

        public static void ScanProjectForFiles(Project project)
        {
            lock (_projectsDictionary)
            {
                var filter = EssentialProperties.SolutionFilter;

                // check if cache is still current according to filter used
                if (_lastFilter != filter)
                {
                    _projectsDictionary.Clear();
                    //_folderDictionary.Clear();
                    _treeDictionary.Clear();
                    _lastFilter = filter;
                }

                // check if cache is still current
                DateTime lastDate;
                if (_projectsDictionary.TryGetValue(project.Name, out lastDate))
                {
                    var d = DateTime.Now - lastDate;
                    if (d.TotalSeconds < 60)
                        return;
                }
                _projectsDictionary[project.Name] = DateTime.Now;
                var filterArray = EssentialProperties.SolutionFilterArray;

                // clear all entries related to project.
                ClearCacheOfProject(project);

                // Look for files that are in this project and comply with the filter. Add them to dictionary
                foreach (var file in project.Files)
                {
                    FilePath path;

                    if (!file.Visible || file.Flags.HasFlag(ProjectItemFlags.Hidden) || file.Subtype == Subtype.Directory)
                        continue;

                    path = file.IsLink ? project.BaseDirectory.Combine(file.ProjectVirtualPath) : file.FilePath;

                    foreach (var key in filterArray)
                    {

                        if (file.ProjectVirtualPath.ToString().ToLower().Contains(key))
                        {
                            // Found file of folder that fits the filter
                            var filenameInFilter = false;
                            var filenameToTest = file.FilePath.FileName.ToLower();
                            foreach (var key1 in filterArray)
                            {
                                if (filenameToTest.Contains(key1))
                                {
                                    filenameInFilter = true;
                                    break;
                                }
                            }

                            var filename = file.ProjectVirtualPath.FileName;
                            var folder = file.ProjectVirtualPath.ParentDirectory;
                            RegisterFile(project.Name, filename, folder, filenameInFilter, filterArray);



                            //if (folder == null || folder.IsEmpty) // Register a project's root file
                            //{
                            //    var foldername = project.Name;
                            //    List<string> list;
                            //    if (!_folderDictionary.TryGetValue(foldername, out list))
                            //        list = new List<string> { filename };
                            //    else
                            //        list.Add(filename);
                            //    _folderDictionary[foldername] = list;
                            //}
                            //else
                            //    RegisterFileForFolder(project.Name, filename, folder);

                            break;
                        }
                    }
                }
            }
            Debug.WriteLine(ToString());
        }

        private static void ClearCacheOfProject(Project project)
        {
            //foreach (var s in _folderDictionary.Where(item => item.Key.StartsWith(project.Name)).ToList())
            //{
            //    _folderDictionary.Remove(s.Key);
            //}

            foreach (var s in _treeDictionary.Where(item => item.Key.StartsWith(project.Name)).ToList())
            {
                _treeDictionary.Remove(s.Key);
            }
        }

        ///// <summary>
        ///// Recursive routine which registers an entry for each folder in the file's path 
        ///// </summary>
        //private string RegisterFileForFolder(string projectname, string filename, FilePath folder)
        //{
        //    if (folder == null || folder.IsEmpty)
        //        return projectname;

        //    // Enter recursion
        //    var foldername = RegisterFileForFolder(projectname, filename, folder.ParentDirectory) + $"\\" + folder.FileName;

        //    // Register file in dictionary
        //    List<string> list;
        //    if (!_folderDictionary.TryGetValue(foldername, out list))
        //        list = new List<string> { filename };
        //    else
        //        list.Add(filename);
        //    _folderDictionary[foldername] = list;

        //    return foldername;
        //}

        /// <summary>
        /// Recursive routine which registers an entry for each folder in the file's path 
        /// </summary>
        private static void RegisterFile(string projectname, string filename, FilePath folder, bool filenameInFilter, string[] filter)
        {
            RegisterFileForFolder(projectname, filename, folder, filenameInFilter, filter);

            // Register file
            var foldername = projectname + "/" + folder.ToString() + "/" + filename;
            TreeItem item;
            if (!_treeDictionary.TryGetValue(foldername, out item))
                item = new TreeItem { ProjectVirtualPath = folder };

            item.IsExpanded = item.IsExpanded || filenameInFilter;
            _treeDictionary[foldername] = item;
        }

        /// <summary>
        /// Recursive routine which registers an entry for each folder in the file's path 
        /// </summary>
        private static string RegisterFileForFolder(string projectname, string filename, FilePath folder, bool filenameInFilter, string[] filter)
        {
            if (folder == null || folder.IsEmpty)
                return projectname;

            // Enter recursion
            var foldername = RegisterFileForFolder(projectname, filename, folder.ParentDirectory, filenameInFilter, filter) + $"/" + folder.FileName;

            // Register file in dictionary
            TreeItem item;
            if (!_treeDictionary.TryGetValue(foldername, out item))
                item = new TreeItem { ProjectVirtualPath = folder };
            item.IsExpanded = item.IsExpanded || filenameInFilter || IsFilePathExpanded(folder, filter);
            _treeDictionary[foldername] = item;

            return foldername;
        }

        private static bool IsFilePathExpanded(FilePath folder, string[] filter)
        {
            foreach (var key in filter)
            {
                if (folder.FileName.ToLower().Contains(key))
                    return true;
            }
            return false;
        }

        private static string GetFoldername(ProjectFolder folder)
        {
            // Enter recursion
            var parent = folder.Parent;
            if (parent is Project project)
                return project.Name + "/" + folder.Name;

            return GetFoldername(parent as ProjectFolder) + $"/" + folder.Name;
        }


        private static string GetKeyFor(object dataObject)
        {
            switch (dataObject)
            {
                case ProjectFile file:
                    return file.Project.Name + "/" + file.ProjectVirtualPath.ToString();
                case ProjectFolder folder:
                    return GetFoldername(folder);
                case Project project:
                    return project.Name;
                default:
                    return string.Empty;
            }
        }

        //    private static bool IsProjectFolderVisible(ProjectFolder folder)
        //{
        //    //var key = GetFoldername(folder);
        //    //List<string> list;
        //    //return _folderDictionary.TryGetValue(key, out list) && list.Count > 0;

        //    var key = GetFoldername(folder);
        //    return _treeDictionary.TryGetValue(key, out _);
        //}

        //private static bool IsProjectExpanded(ProjectFile file)
        //{
        //    var key = file.Project.Name + "\\" + file.ProjectVirtualPath.ToString();
        //    TreeItem item;
        //    return _treeDictionary.TryGetValue(key, out item) && item.IsExpanded;
        //}

        //private static bool IsProjectFolderExpanded(ProjectFolder folder)
        //{
        //    var key = GetFoldername(folder);
        //    TreeItem item;
        //    return _treeDictionary.TryGetValue(key, out item) && item.IsExpanded;
        //}


        public static bool IsProjectItemVisible(object dataItem)
        {
            if (dataItem is ProjectFile || dataItem is ProjectFolder)
            {
                var key = GetKeyFor(dataItem);
                return _treeDictionary.TryGetValue(key, out _);
            }
            else if (dataItem is Project project)
            {
                return _treeDictionary.Keys.FindIndex((key) => key.Contains(project.Name)) != -1;
            }
            return false;
        }

        public static bool IsProjectItemExpanded(object dataItem)
        {
            if (dataItem is ProjectFile || dataItem is ProjectFolder)
            {
                var key = GetKeyFor(dataItem);
                TreeItem item;
                var found = _treeDictionary.TryGetValue(key, out item);
                return found && item.IsExpanded;
            }
            else if (dataItem is Project project)
            {
                return _treeDictionary.Keys.FindIndex((key) => key.Contains(project.Name)) != -1;
            }
            return false;
        }

        //public static bool IsProjectVisible(Project project)
        //{
        //    var projectname = project.Name.ToLower();
        //    var index = _treeDictionary.Keys.FindIndex((key) => key.ToLower().Contains(projectname));
        //    return index != -1;
        //}

        public static new string ToString()
        {
            var str = "";
            //str += $"{{FilteredProjectCache}}\n";
            //foreach (var item in _folderDictionary)
            //{
            //    str += $"\tFolder: {item.Key}\n";
            //    foreach (var filename in item.Value)
            //    {
            //        str += $"\t\t=> {filename}\n";
            //    }
            //}

            var maxLength = 0;

            foreach (var item in _treeDictionary)
            {
                if (item.Key.Length > maxLength)
                    maxLength = item.Key.Length;
            }
            maxLength += 1;

            str += $"\n{{FilteredProjectCache - TreeDictionary}}\n";
            foreach (var item in _treeDictionary)
            {
                str += $"\t- {item.Key.PadRight(maxLength, '.')}\t=> IsExpanded: {item.Value.IsExpanded}\n";
            }
            return str;
        }
    }
}