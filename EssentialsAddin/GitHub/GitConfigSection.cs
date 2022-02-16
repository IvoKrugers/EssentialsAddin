using System;
using System.Linq;
using System.Collections.Generic;

namespace EssentialsAddin.GitHub
{
    class GitConfigSection
    {
        public string Type { get; set; }
        public string Name { get; set; }

        internal bool Modified { get; set; }

        List<Tuple<string, string>> properties = new List<Tuple<string, string>>();

        public IEnumerable<string> Keys
        {
            get { return properties.Select(p => p.Item1); }
        }

        public string GetValue(string key)
        {
            return properties.Where(p => p.Item1 == key).Select(p => p.Item2).FirstOrDefault();
        }

        public void SetValue(string key, string value)
        {
            var i = properties.FindIndex(t => t.Item1 == key);
            if (i == -1)
            {
                var p = new Tuple<string, string>(key, value);
                properties.Add(p);
                Modified = true;
            }
            else
            {
                if (properties[i].Item2 != value)
                {
                    properties[i] = new Tuple<string, string>(key, value);
                    Modified = true;
                }
            }
        }

        public void RemoveValue(string key)
        {
            var i = properties.FindIndex(t => t.Item1 == key);
            if (i != -1)
            {
                properties.RemoveAt(i);
                Modified = true;
            }
        }
    }
}

