using System;
using Newtonsoft.Json;

namespace EssentialsAddin.Services
{
    public partial class Release
    {
        [JsonProperty("html_url")]
        public Uri Url { get; set; }

        [JsonProperty("tag_name")]
        public string TagName { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
    }
}
