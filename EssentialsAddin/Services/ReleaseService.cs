using System;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EssentialsAddin.Services
{
    public class ReleaseService
    {
        public Release LatestRelease { get; private set; }

        internal static class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                //Converters =
                //{
                //    new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
                //},
            };
        }

        private async Task<Release> GetLatestReleaseAsync()
        {
            var uri = new Uri("http://api.github.com/repos/IvoKrugers/EssentialsAddin/releases/latest");
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");//Set the User Agent to "request"

                using (HttpResponseMessage response = await client.GetAsync(uri))
                {
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Release>(result, Converter.Settings);
                }
            }
        }
        private async Task GetLatestReleaseFromAddinRepo()
        {
            var uri = new Uri("https://raw.githubusercontent.com/IvoKrugers/EssentialsAddin/develop/mpack/EssentialsAddin/main.mrep");
        }

        public async Task<bool> CheckForNewRelease()
        {
            try
            {
                LatestRelease = await GetLatestReleaseAsync();
                return LatestRelease.TagName != Constants.Version;
            }
            catch (Exception)
            {
                LatestRelease = new Release();
                return false;
            }
           
        }
    }
}
