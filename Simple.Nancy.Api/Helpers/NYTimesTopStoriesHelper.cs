using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Simple.Nancy.Api.Models.DTO;
using Simple.Nancy.Api.Models.ViewModels;

namespace Simple.Nancy.Api.Helpers
{
    public class NYTimesTopStoriesHelper : INYTimesTopStoriesHelper
    {
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


            if (model.Articles.Equals(null))
            {
                throw new NullReferenceException("Something went wrong, try again later or provide different input");

            }

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
