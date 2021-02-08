using System.Net.Http;
using System.Threading.Tasks;

namespace Simple.Nancy.Api.Helpers
{
    public class NyTimesTopStoriesApiCaller : INyTimesTopStoriesApiCaller
    {
        //Unfortunately i have to use this, for some reason httpclient.BaseAddress is not setting up in Startup.cs, maybe its Nancy related issues
        private readonly string mainUrl = "https://api.nytimes.com/svc/topstories/v2/";
        private readonly HttpClient _httpClient;
        private readonly string apiKey = "api-key=k0XA0k0jJGAVuv8Jr5wAIcKDGPuznmRJ";

        public NyTimesTopStoriesApiCaller(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> CallSpecificTopicSectionAsync(string section)
        {
            var requestUrl = mainUrl + $"{section}.json?{apiKey}";

            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> CallDefaultTopicSectionAsync()
        {
            var requestUrl = mainUrl + $"home.json?{apiKey}";

            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}
