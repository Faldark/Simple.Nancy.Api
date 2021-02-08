using System;
using Newtonsoft.Json;

namespace Simple.Nancy.Api.Models.ViewModels
{
    public class ArticleView
    {
        [JsonProperty(PropertyName = "title")]
        public string Heading { get; set; }
        [JsonProperty(PropertyName = "updated_date")]
        public DateTime Updated { get; set; }
        [JsonProperty(PropertyName = "short_url")]
        public string Link { get; set; }
    }
}
