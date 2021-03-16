using System;
using System.Collections.Generic;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Projects;
using Newtonsoft.Json;

namespace EssentialsAddin.Helpers
{
    public class PropertyService
    {
        const string FILE_NAME = "EssentialsAddin.json";

        private static PropertyService _instance;
        public static PropertyService Instance => _instance ?? (_instance = new PropertyService());

        private string _filePath;
        private readonly JsonSerializer _serializer;

        private Dictionary<string, string> _properties;

        PropertyService()
        {
            _serializer = new JsonSerializer();
        }

        public void Init(Solution solution)
        {
            if (solution == null)
                return;
            EnsurePropertyFile(solution);
            ReadProperties();
        }

        public T Get<T>(string key, T defaultValue) where T : IConvertible
        {
            string value = Get(key, defaultValue.ToString());
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public void Set<T>(string key, T value) where T : IConvertible
        {
            string strValue = (string)Convert.ChangeType(value, typeof(string));
            Set(key, strValue);
        }

        public string Get(string key, string defaultValue = "")
        {
            if (_properties == null)
                return defaultValue;

            return (_properties.ContainsKey(key) ? _properties[key] : defaultValue);
        }

        public void Set(string key, string value)
        {
            _properties[key] = value;
            WriteProperties();
        }

        private void ReadProperties()
        {
            using (StreamReader reader = File.OpenText(_filePath))
            {
                var data = (Dictionary<string, string>)_serializer.Deserialize(reader, typeof(Dictionary<string, string>));
                _properties = data ?? new Dictionary<string, string>();
            }
        }

        private void WriteProperties()
        {
            using (StreamWriter writer = File.CreateText(_filePath))
            {
                _serializer.Serialize(writer, _properties);
            }
        }

        private void EnsurePropertyFile(Solution solution)
        {
            var solutionPath = solution.BaseDirectory.ToAbsolute(new FilePath());
            _filePath = Path.Combine(solutionPath, FILE_NAME);

            if (!File.Exists(_filePath))
                File.CreateText(_filePath).Close();
        }
    }
}