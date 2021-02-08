using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Simple.Nancy.Api.Helpers
{
    public class NyTimesTopStoriesApiCaller
    {
        private HttpClient Client { get; set; }
        private readonly string mainUrl = "https://api.nytimes.com/svc/topstories/v2/";
        private readonly string apiKey = "api-key=k0XA0k0jJGAVuv8Jr5wAIcKDGPuznmRJ";
        private readonly IConfiguration Configuration;

        public  NyTimesTopStoriesApiCaller(IConfiguration configuration)
        {
            Configuration = configuration;
            Client = new HttpClient();
        }

        public async Task<HttpResponseMessage> CallSpecificTopicSectionAsync(string section)
        {
            var requestUrl = mainUrl + $"{section}.json?{apiKey}";
            using (Client)
            {
                return await Client.GetAsync(requestUrl);
            }
        }

        public async Task<HttpResponseMessage> CallDefaultTopicSectionAsync()
        {
            var requestUrl = mainUrl + $"home.json?{apiKey}";
            using (Client)
            {
                return await Client.GetAsync(requestUrl);
            }
        }
    }
}
