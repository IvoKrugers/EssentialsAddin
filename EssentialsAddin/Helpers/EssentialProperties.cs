using System;
using MonoDevelop.Core;
using MonoDevelop.Core.Collections;

namespace EssentialsAddin.Helpers
{
    public static class EssentialProperties
    {
        private const string SOLUTIONFILTER_KEY = "EssentialsAddin.SolutionFilter";
        private const string SOLUTIONEXPANDFILTER_KEY = "EssentialsAddin.SolutionExpandFilter";

        private static char[] _delimiterChars = { ' ', ';', ':', '\t', '\n' };

        public static string SolutionFilter
        {
            get => PropertyService.Get<string>(SOLUTIONFILTER_KEY, String.Empty);
            set => PropertyService.Set(SOLUTIONFILTER_KEY, value.ToLower());
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
            get => PropertyService.Get<string>(SOLUTIONEXPANDFILTER_KEY, String.Empty);
            set => PropertyService.Set(SOLUTIONEXPANDFILTER_KEY, value.ToLower());
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

        public static string[] ExcludedExtensionsFromOneClick = { ".storyboard", ".xib" };

        public static string[] ExcludedExtensionsFromExpanding = { ".xaml.cs", ".designer.cs" };
    }
}
