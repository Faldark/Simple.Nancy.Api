using Newtonsoft.Json;
using System.Collections.Generic;
using Simple.Nancy.Api.Models.ViewModels;

namespace Simple.Nancy.Api.Models.DTO
{
    public class NYTopStoriesModel
    {
        [JsonProperty(PropertyName = "results")]
        public IList<ArticleView> Articles { get; set; }
    }
}
