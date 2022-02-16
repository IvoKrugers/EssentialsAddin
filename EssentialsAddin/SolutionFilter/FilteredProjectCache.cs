using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EssentialsAddin.Helpers;
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
            public bool IsEnabled;
        }

        private static Dictionary<string, DateTime> _projectsDictionary = new Dictionary<string, DateTime>();
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
                var pinnedDocuments = EssentialProperties.PinnedDocuments;

                // clear all entries related to project.
                ClearCacheOfProject(project);

                // Look for files that are in this project and comply with the filter. Add them to dictionary
                foreach (var file in project.Files)
                {
                    FilePath path;

                    //If the file is pinned in the Workbench, add it to the cache.
                    if (filterArray.Count() > 0 && pinnedDocuments.Contains(file.FilePath))
                        RegisterFile(project.Name, file.ProjectVirtualPath.FileName, file.ProjectVirtualPath.ParentDirectory, true, filterArray, true);

                    if (!file.Visible || file.Flags.HasFlag(ProjectItemFlags.Hidden) || file.Subtype == Subtype.Directory)
                        continue;

                    path = file.IsLink ? project.BaseDirectory.Combine(file.ProjectVirtualPath) : file.FilePath;

                    if (filterArray.Length > 0)
                    {
                        foreach (var key in filterArray)
                        {

                            if (file.ProjectVirtualPath.ToString().ToLower().Contains(key))
                            {
#if DEBUG
                                if (file.FilePath.FileName == "StageProgressBar.designer.cs")
                                {
                                    Debug.WriteLine("BINGO !!");
                                }
#endif


                                // if this file depends on another, make sure that that parent file in the cache is marked as expanded too.


                                // Found file or folder that fits the filter
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

                                break;
                            }
                        }
                    }
                }
            }
            Debug.WriteLine(ToString());
        }

        private static void ClearCacheOfProject(Project project)
        {
            foreach (var s in _treeDictionary.Where(item => item.Key.StartsWith(project.Name)).ToList())
            {
                _treeDictionary.Remove(s.Key);
            }
        }

        /// <summary>
        /// Recursive routine which registers an entry for each folder in the file's path 
        /// </summary>
        private static void RegisterFile(string projectname, string filename, FilePath folder, bool filenameInFilter, string[] filter, bool isPinnedDoc = false)
        {
            RegisterFileForFolder(projectname, filename, folder, filenameInFilter, filter);

            // Register file
            var foldername = projectname + "/" + folder.ToString() + "/" + filename;
            foldername = foldername.Replace("//", "/");
            TreeItem item;
            if (!_treeDictionary.TryGetValue(foldername, out item))
                item = new TreeItem { ProjectVirtualPath = folder };

            item.IsExpanded = item.IsExpanded || filenameInFilter;
            item.IsEnabled = item.IsEnabled || filenameInFilter;
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
            foldername = foldername.Replace("//", "/");
            // Register file in dictionary
            TreeItem item;
            if (!_treeDictionary.TryGetValue(foldername, out item))
                item = new TreeItem { ProjectVirtualPath = folder };
            item.IsExpanded = item.IsExpanded || filenameInFilter || IsFilePathExpanded(folder, filter);
            item.IsEnabled = true;
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
            var parent = folder.Parent;
            if (parent is Project project)
                return project.Name + "/" + folder.Name;

            // Enter recursion
            return GetFoldername(parent as ProjectFolder) + $"/" + folder.Name;
        }

        private static string GetKeyFor(object dataObject)
        {
            string key;
            switch (dataObject)
            {
                case ProjectFile file:
                    key = file.Project.Name + "/" + file.ProjectVirtualPath.ToString();
                    break;
                case ProjectFolder folder:
                    key = GetFoldername(folder);
                    break;
                case Project project:
                    key = project.Name;
                    break;
                default:
                    key = string.Empty;
                    break;
            }
            key = key.Replace("//", "/");
            return key;
        }

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

        public static bool IsProjectItemEnabled(object dataItem)
        {
            if (dataItem is ProjectFile || dataItem is ProjectFolder)
            {
                var key = GetKeyFor(dataItem);
                TreeItem item;
                if (!_treeDictionary.TryGetValue(key, out item))
                    return false;
                return item.IsEnabled;
            }
            else if (dataItem is Project project)
            {
                return true;
            }
            return false;
        }

        public static bool IsProjectItemExpanded(object dataItem)
        {
            if (dataItem is ProjectFile || dataItem is ProjectFolder)
            {
                var key = GetKeyFor(dataItem);
                TreeItem item;

                if (dataItem is ProjectFile pf && pf.DependentChildren.Where(f => f.FilePath.FileName.EndsWith(".designer.cs")).Any())
                    return false;

                var found = _treeDictionary.TryGetValue(key, out item);
                return found && item.IsExpanded;
            }
            else if (dataItem is Project project)
            {
                return _treeDictionary.Keys.FindIndex((key) => key.Contains(project.Name)) != -1;
            }
            return false;
        }

        public static new string ToString()
        {
            var str = "";
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
                str += $"\t- {item.Key.PadRight(maxLength, '.')}\t=> IsExpanded: {item.Value.IsExpanded}\t=> IsEnabled: {item.Value.IsEnabled}\n";
            }
            return str;
        }
    }
}