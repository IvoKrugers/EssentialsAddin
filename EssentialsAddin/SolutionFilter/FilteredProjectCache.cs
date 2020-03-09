using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cairo;
using EssentialsAddin.Helpers;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Pads.ProjectPad;
using MonoDevelop.Projects;

namespace EssentialsAddin.SolutionFilter
{
    public class FilteredProjectCache
    {
        private Dictionary<string, DateTime> _projectsDictionary = new Dictionary<string, DateTime>();
        private Dictionary<string, List<string>> _folderDictionary = new Dictionary<string, List<string>>();

        private string _lastFilter = string.Empty;

        public void ScanProjectForFiles(Project project)
        {
            var filter = EssentialProperties.SolutionFilter;
            // check if cache is still current according to filter used
            if (_lastFilter != filter)
            {
                _projectsDictionary.Clear();
                _folderDictionary.Clear();
                _lastFilter = filter;
            }

            // check if cache is still current
            DateTime lastDate;
            if (_projectsDictionary.TryGetValue(project.Name, out lastDate))
            {
                var d = DateTime.Now - lastDate;
                if (d.TotalSeconds < 30)
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
                        var filename = file.ProjectVirtualPath.FileNameWithoutExtension.ToLower();
                        var folder = file.ProjectVirtualPath.ParentDirectory;
                        RegisterFileForFolder(project.Name, filename, folder);
                        break;
                    }
                }
            }

            Debug.WriteLine(ToString());
        }

        private void ClearCacheOfProject(Project project)
        {
            foreach (var s in _folderDictionary.Where(item => item.Key.StartsWith(project.Name)).ToList())
            {
                _folderDictionary.Remove(s.Key);
            }
        }

        public override string ToString()
        {
            var str = "";
            str += $"{{FilteredProjectCache}}\n";
            foreach (var item in _folderDictionary)
            {
                str += $"\tFolder: {item.Key}\n";
                foreach (var filename in item.Value)
                {
                    str += $"\t\t=> {filename}\n";
                }
            }
            return str;
        }

        private string RegisterFileForFolder(string projectname, string filename, FilePath folder)
        {
            if (folder == null || folder.IsEmpty)
                return projectname;

            // Enter recursion
            var foldername = RegisterFileForFolder(projectname, filename, folder.ParentDirectory) + $"\\" + folder.FileName;

            // Register file in dictionary
            List<string> list;
            if (!_folderDictionary.TryGetValue(foldername, out list))
                list = new List<string> { filename };
            else
                list.Add(filename);
            _folderDictionary[foldername] = list;

            return foldername;
        }

        //private string RegisterFolder(string projectname, FilePath folder)
        //{
        //    if (folder == null || folder.IsEmpty)
        //        return projectname;

        //    // Enter recursion
        //    var foldername = RegisterFolder(projectname, folder.ParentDirectory) + $"\\" + folder.FileName;

        //    // Register file in dictionary
        //    List<string> list;
        //    if (!_folderDictionary.TryGetValue(foldername, out list))
        //        list = new List<string> { filename };
        //    else
        //        list.Add(filename);
        //    _folderDictionary[foldername] = list;

        //    return foldername;
        //}

        private string GetFoldername(ProjectFolder folder)
        {
            // Enter recursion
            var parent = folder.Parent;
            if (parent is Project project)
                return project.Name + "\\" + folder.Name;

            return GetFoldername(parent as ProjectFolder) + $"\\" + folder.Name;
        }

        public bool IsProjectFolderVisible(ProjectFolder folder)
        {
            var key = GetFoldername(folder);
            List<string> list;
            return _folderDictionary.TryGetValue(key, out list) && list.Count > 0;
        }
    }
}