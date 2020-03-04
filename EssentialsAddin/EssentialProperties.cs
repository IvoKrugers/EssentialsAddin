using System;
using MonoDevelop.Core;

namespace EssentialsAddin
{
    public static class EssentialProperties
    {
        private static char[] _delimiterChars = { ' ', ';', ':', '\t', '\n' };

        public static string SolutionFilter
        {
            get => PropertyService.Get<string>(SolutionFilterPad.PROPERTY_KEY, String.Empty);
            set => PropertyService.Set(SolutionFilterPad.PROPERTY_KEY, value.ToLower());
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

    }
}
