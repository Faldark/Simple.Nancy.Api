using System.Net.Http;
using System.Threading.Tasks;

namespace Simple.Nancy.Api.Helpers
{
    public interface INyTimesTopStoriesApiCaller
    {
        public Task<HttpResponseMessage> CallSpecificTopicSectionAsync(string section);
        public Task<HttpResponseMessage> CallDefaultTopicSectionAsync();
    }
}
