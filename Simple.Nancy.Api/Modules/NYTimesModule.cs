using Nancy;
using Newtonsoft.Json;
using Simple.Nancy.Api.Helpers;
using Simple.Nancy.Api.Models.DTO;
using System;
using System.Linq;
using System.Text.Json;
using Simple.Nancy.Api.Models.ViewModels;

namespace Simple.Nancy.Api.Modules
{
    public class NYTimesModule : NancyModule
    {
        private static readonly JsonSerializerOptions SerializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public NYTimesModule(INyTimesTopStoriesApiCaller nyTimesTopStoriesApiCaller)
        {
            INyTimesTopStoriesApiCaller nYTimesTopStoriesApiCaller = nyTimesTopStoriesApiCaller;

            Get("/", args => {

                var result = "This api is working fine";
                var response = (Response)result;
                response.ContentType = "application/json";
                response.StatusCode = HttpStatusCode.OK;

                return response;
            });

            Get("/list/{section}", async args =>
            {
                var res = await nYTimesTopStoriesApiCaller.CallSpecificTopicSectionAsync(args.section);
                var result = await res.Content.ReadAsStringAsync();

                NYTopStoriesModel model = JsonConvert.DeserializeObject<NYTopStoriesModel>(result);

                return $"{System.Text.Json.JsonSerializer.Serialize(model.Articles, SerializeOptions)}";
            });

            Get("/list/{section}/first", async args =>
            {
                var res = await nYTimesTopStoriesApiCaller.CallSpecificTopicSectionAsync(args.section);
                var result = await res.Content.ReadAsStringAsync();

                NYTopStoriesModel model = JsonConvert.DeserializeObject<NYTopStoriesModel>(result);

                return $"{System.Text.Json.JsonSerializer.Serialize(model.Articles.First(), SerializeOptions)}";
            });


            Get("/list/{section}/{updatedDate}", async args =>
            {
                if (!DateTime.TryParse(args.updatedDate, out DateTime updatedDate))
                {
                    throw new FormatException("Wrong date format, advised format is yyyy-MM-dd, example: 1992-12-31");
                }

                var res = await nYTimesTopStoriesApiCaller.CallSpecificTopicSectionAsync(args.section);
                var result = await res.Content.ReadAsStringAsync();

                NYTopStoriesModel model = JsonConvert.DeserializeObject<NYTopStoriesModel>(result);

                return $"{System.Text.Json.JsonSerializer.Serialize(model.Articles.Where(a => a.Updated.Date == updatedDate.Date), SerializeOptions)}";
            });

            Get("/article/{shortUrl}", async args =>
            {
                var res = await nYTimesTopStoriesApiCaller.CallDefaultTopicSectionAsync();
                var content = await res.Content.ReadAsStringAsync();

                NYTopStoriesModel model = JsonConvert.DeserializeObject<NYTopStoriesModel>(content);

                return $"{System.Text.Json.JsonSerializer.Serialize(model.Articles.Single(a => a.Link.EndsWith(args.shortUrl)), SerializeOptions)}";
            });

            Get("/group/{section}", async args =>
            {
                var res = await nYTimesTopStoriesApiCaller.CallSpecificTopicSectionAsync(args.section);
                var result = await res.Content.ReadAsStringAsync();

                NYTopStoriesModel model = JsonConvert.DeserializeObject<NYTopStoriesModel>(result);
                
                var groupedModel = model.Articles.GroupBy(a => a.Updated.Date).Select(x => new ArticleGroupByDateView
                {
                    Date = x.Key.Date.ToString("yyyy-MM-dd"), Total = x.Count()
                });
                
                return $"{System.Text.Json.JsonSerializer.Serialize(groupedModel, SerializeOptions)}";
            });
        }
    }
}