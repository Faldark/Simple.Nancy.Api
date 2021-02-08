using System.Collections.Generic;
using System.Threading.Tasks;
using Simple.Nancy.Api.Models.ViewModels;

namespace Simple.Nancy.Api.Helpers
{
    public interface INYTimesTopStoriesHelper
    {
        public Task<IList<ArticleView>> GetSpecificSectionArticles(string section);
        public Task<IList<ArticleView>> GetDefaultSectionArticles();
    }
}
