using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Simple.Nancy.Api.Models.DTO;
using Simple.Nancy.Api.Models.ViewModels;

namespace Simple.Nancy.Api.Helpers
{
    public class NYTimesTopStoriesHelper : INYTimesTopStoriesHelper
    {
        private static readonly JsonSerializerOptions SerializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        private readonly INyTimesTopStoriesApiCaller _nYTimesTopStoriesApiCaller;

        public NYTimesTopStoriesHelper(INyTimesTopStoriesApiCaller nYTimesTopStoriesApiCaller)
        {
            _nYTimesTopStoriesApiCaller = nYTimesTopStoriesApiCaller;
        }
        
        public async Task<IList<ArticleView>> GetSpecificSectionArticles(string section)
        {
            var res = await _nYTimesTopStoriesApiCaller.CallSpecificTopicSectionAsync(section);
            var result = await res.Content.ReadAsStringAsync();

            NYTopStoriesModel model = JsonConvert.DeserializeObject<NYTopStoriesModel>(result);

            return model.Articles;
        }

        public async Task<IList<ArticleView>> GetDefaultSectionArticles()
        {
            var res = await _nYTimesTopStoriesApiCaller.CallDefaultTopicSectionAsync();
            var result = await res.Content.ReadAsStringAsync();

            NYTopStoriesModel model = JsonConvert.DeserializeObject<NYTopStoriesModel>(result);

            return model.Articles;
        }
    }
}
