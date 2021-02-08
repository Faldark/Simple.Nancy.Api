using System.Net.Http;
using System.Threading.Tasks;

namespace Simple.Nancy.Api.Helpers
{
    public class NyTimesTopStoriesApiCaller : INyTimesTopStoriesApiCaller
    {
        private HttpClient Client { get; }
        private readonly string mainUrl = "https://api.nytimes.com/svc/topstories/v2/";
        private readonly string apiKey = "api-key=k0XA0k0jJGAVuv8Jr5wAIcKDGPuznmRJ";

        public  NyTimesTopStoriesApiCaller()
        {
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
