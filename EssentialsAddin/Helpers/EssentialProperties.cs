using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Utilities.Internal;

namespace EssentialsAddin.Helpers
{
    public static class EssentialProperties
    {
        private const string SOLUTIONFILTER_KEY = "EssentialsAddin.SolutionFilter";
        private const string SOLUTIONOPENDOCUMENTS_KEY = "EssentialsAddin.SolutionOpenDocuments";
        private const string SOLUTIONEXPANDFILTER_KEY = "EssentialsAddin.SolutionExpandFilter";
        private const string ONECLICKSHOWFILE_KEY = "EssentialsAddin.OneClickShowFile";
        private const string CONSOLEFILTER_KEY = "EssentialsAddin.ConsoleFilter";

        private static char[] _delimiterChars = { ' ', ';', ':', '\t', '\n' };

        public static string SolutionFilter
        {
            get => PropertyService.Instance.Get(SOLUTIONFILTER_KEY, String.Empty);
            set => PropertyService.Instance.Set(SOLUTIONFILTER_KEY, value.ToLower());
        }

        public static void ClearOpenDocuments()
        {
            OpenDocuments = new List<string>();
        }

        public static bool AddOpenDocument(MonoDevelop.Ide.Gui.Document document)
        {
            var path = document.FilePath.FullPath;
            var filenames = OpenDocuments;

            if (!filenames.Contains(path))
            {
                filenames.Add(path);
                OpenDocuments = filenames;
                return true;
            }
            return false;
        }

        public static bool RemoveOpenDocument(MonoDevelop.Ide.Gui.Document document)
        {
            var path = document.FilePath.FullPath;
            var filenames = OpenDocuments;

            if (filenames.Contains(path))
            {
                filenames.Remove(path);
                OpenDocuments = filenames;
                return true;
            }
            return false;
        }

        public static List<string> OpenDocuments
        {
            get
            {
                var commaSepString = PropertyService.Instance.Get(SOLUTIONOPENDOCUMENTS_KEY, "");
                if (string.IsNullOrEmpty(commaSepString))
                    return new List<string>();
                char[] _delimiter = { ',' };
                return commaSepString.Split(_delimiter).ToList();
            }
            set => PropertyService.Instance.Set(SOLUTIONOPENDOCUMENTS_KEY, value.Join(","));
        }

        public static string[] SolutionFilterArray
        {
            get
            {
                var filterText = SolutionFilter;
                //filterText = "items".ToLower();
                if (string.IsNullOrEmpty(filterText))
                    return new string[0];

                return filterText.Split(_delimiterChars);
            }
        }

        public static string ExpandFilter
        {
            get => PropertyService.Instance.Get(SOLUTIONEXPANDFILTER_KEY, string.Empty);
            set => PropertyService.Instance.Set(SOLUTIONEXPANDFILTER_KEY, value.ToLower());
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
            get => PropertyService.Instance.Get(CONSOLEFILTER_KEY, string.Empty);
            set => PropertyService.Instance.Set(CONSOLEFILTER_KEY, value.ToLower());
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
    }
}