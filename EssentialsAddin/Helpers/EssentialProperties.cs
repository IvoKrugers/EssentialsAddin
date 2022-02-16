using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EssentialsAddin.GitHub;
using Microsoft.VisualStudio.Utilities.Internal;

namespace EssentialsAddin.Helpers
{
    public static class EssentialProperties
    {
        private const string SOLUTIONFILTER_KEY = "SolutionFilter";
        private const string SOLUTIONPINNEDDOCUMENTS_KEY = "SolutionPinnedDocuments";
        private const string SOLUTIONEXPANDFILTER_KEY = "SolutionExpandFilter";
        private const string ONECLICKSHOWFILE_KEY = "OneClickShowFile";
        private const string CONSOLEFILTER_KEY = "ConsoleFilter";

        private static char[] _delimiterChars = { ' ', ';', ':', '\t', '\n' };

        public static bool Initialized => PropertyService.Instance.Initialized;

        public static string SolutionFilter
        {
            get => Get(SOLUTIONFILTER_KEY, String.Empty);
            set => Set(SOLUTIONFILTER_KEY, value.ToLower());
        }

        public static void ClearPinnedDocuments()
        {
            PinnedDocuments = new List<string>();
        }

        public static bool AddPinnedDocument(MonoDevelop.Ide.Gui.Document document)
        => AddPinnedDocument(document.FilePath.FullPath);

        public static bool AddPinnedDocument(MonoDevelop.Projects.ProjectFile projectFile)
        => AddPinnedDocument(projectFile.FilePath.FullPath);

        private static bool AddPinnedDocument(string fullFilePath)
        {
            var filenames = PinnedDocuments;

            if (!filenames.Contains(fullFilePath))
            {
                filenames.Add(fullFilePath);
                PinnedDocuments = filenames;
                return true;
            }
            return false;
        }

        public static bool RemovePinnedDocument(MonoDevelop.Ide.Gui.Document document)
         => RemovePinnedDocument(document.FilePath.FullPath);

        public static bool RemovePinnedDocument(MonoDevelop.Projects.ProjectFile projectFile)
        => RemovePinnedDocument(projectFile.FilePath.FullPath);

        private static bool RemovePinnedDocument(string fullFilePath)
        {
            var filenames = PinnedDocuments;

            if (filenames.Contains(fullFilePath))
            {
                filenames.Remove(fullFilePath);
                PinnedDocuments = filenames;
                return true;
            }
            return false;
        }

        public static List<string> PinnedDocuments
        {
            get
            {
                var commaSepString = Get(SOLUTIONPINNEDDOCUMENTS_KEY, "");
                if (string.IsNullOrEmpty(commaSepString))
                    return new List<string>();
                char[] _delimiter = { ',' };
                return commaSepString.Split(_delimiter).ToList();
            }
            set => Set(SOLUTIONPINNEDDOCUMENTS_KEY, value.Join(","));
        }

        public static bool IsPinned(MonoDevelop.Projects.ProjectFile projectFile)
        => PinnedDocuments.Contains(projectFile.FilePath.FullPath);
            

        private static string BranchnameToKey(string branchName)
        {
            return branchName.Trim()
                            .Replace("  ", " ")
                            .Replace(" ", "-")
                            .Replace("/", "_");
        }

        private static string ConcatGitBranchName(string key)
        {
            var branchName = GitHelper.GetCurrentBranch() ?? "";
            branchName = BranchnameToKey(branchName);

            return $"{key}{(string.IsNullOrEmpty(branchName) ? "" : $"_{ branchName}")}";
        }

        private static void Set(string key, string value)
        {
            PropertyService.Instance.Set(ConcatGitBranchName(key), value);
            PropertyService.Instance.Set(key, value);
        }

        private static string Get(string key, string defaultValue)
        {
            var result = PropertyService.Instance.Get(ConcatGitBranchName(key), defaultValue);
            if (result == defaultValue)
                result = PropertyService.Instance.Get(key, defaultValue);
            return result;
        }

        public static string[] SolutionFilterArray
        {
            get
            {
                var filterText = SolutionFilter;
                if (string.IsNullOrWhiteSpace(filterText))
                    return new string[0];

                filterText = filterText.Trim();
                while (filterText.IndexOf("  ") >= 0)
                {
                    filterText = filterText.Replace("  ", " ");
                }

                return filterText.Split(_delimiterChars);
            }
        }

        public static string ExpandFilter
        {
            get => Get(SOLUTIONEXPANDFILTER_KEY, string.Empty);
            set => Set(SOLUTIONEXPANDFILTER_KEY, value.ToLower());
        }

        public static string[] ExpandFilterArray
        {
            get
            {
                var filterText = ExpandFilter;
                if (string.IsNullOrEmpty(filterText))
                    return new string[0];

                return filterText.Split(_delimiterChars);
            }
        }

        public static string[] ExcludedExtensionsFromOneClick = { ".storyboard", ".xib", ".png", ".ttf" };

        public static string[] ExcludedExtensionsFromExpanding = { ".xaml.cs", ".designer.cs" };

        public static bool OneClickShowFile
        {
            get => PropertyService.Instance.Get(ONECLICKSHOWFILE_KEY, true);
            set => PropertyService.Instance.Set(ONECLICKSHOWFILE_KEY, value);
        }

        public static bool IsRefreshingTree { get; set; }

        public static string ConsoleFilter
        {
            get => Get(CONSOLEFILTER_KEY, string.Empty);
            set => Set(CONSOLEFILTER_KEY, value.ToLower());
        }

        public static string[] ConsoleFilterArray
        {
            get
            {
                var filterText = ConsoleFilter;
                if (string.IsNullOrEmpty(filterText))
                    return new string[0];

                return filterText.Split(_delimiterChars);
            }
        }

        public static void PurgeProperties()
        {
            var keys = PropertyService.Instance.GetAllKeys() ?? new List<string>();
            var branches = GitHelper.GetLocalBranches() ?? new List<string>();
            branches = branches.Select(b => BranchnameToKey(b)).ToList();

            foreach (var key in keys)
            {
                if (branches.FirstOrDefault(b => key.Contains(b)) is null)
                {
                    PropertyService.Instance.RemoveKey(key);
                }
            }
            PropertyService.Instance.WriteProperties();
        }
    }
}