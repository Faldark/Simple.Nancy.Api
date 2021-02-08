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

        public NYTimesModule()
        {
            var nyTimesTopStoriesApiCaller = new NyTimesTopStoriesApiCaller();

            Get("/", args => {

                var result = "This api is working fine";
                var response = (Response)result;
                response.ContentType = "application/json";
                response.StatusCode = HttpStatusCode.OK;

                return response;
            });

            Get("/list/{section}", async args =>
            {
                var res = await nyTimesTopStoriesApiCaller.CallSpecificTopicSectionAsync(args.section);
                res.EnsureSuccessStatusCode();
                var result = await res.Content.ReadAsStringAsync();

                NYTopStoriesModel model = JsonConvert.DeserializeObject<NYTopStoriesModel>(result);

                return $"Hello {System.Text.Json.JsonSerializer.Serialize(model.Articles, SerializeOptions)}";
            });

            Get("/list/{section}/first", async args =>
            {
                var res = await nyTimesTopStoriesApiCaller.CallSpecificTopicSectionAsync(args.section);
                res.EnsureSuccessStatusCode();
                var result = await res.Content.ReadAsStringAsync();

                NYTopStoriesModel model = JsonConvert.DeserializeObject<NYTopStoriesModel>(result);

                return $"Hello {System.Text.Json.JsonSerializer.Serialize(model.Articles.First(), SerializeOptions)}";
            });


            Get("/list/{section}/{updatedDate}", async args =>
            {
                if (!DateTime.TryParse(args.updatedDate, out DateTime updatedDate))
                {
                    throw new FormatException("Wrong date format, advised format is yyyy-MM-dd, example: 1992-12-31");
                }

                var res = await nyTimesTopStoriesApiCaller.CallSpecificTopicSectionAsync(args.section);
                res.EnsureSuccessStatusCode();
                var result = await res.Content.ReadAsStringAsync();

                NYTopStoriesModel model = JsonConvert.DeserializeObject<NYTopStoriesModel>(result);

                return $"Hello {System.Text.Json.JsonSerializer.Serialize(model.Articles.Where(a => a.Updated.Date == updatedDate.Date), SerializeOptions)}";
            });

            Get("/article/{shortUrl}", async args =>
            {
                var res = await nyTimesTopStoriesApiCaller.CallDefaultTopicSectionAsync();
                res.EnsureSuccessStatusCode();
                var content = await res.Content.ReadAsStringAsync();

                NYTopStoriesModel model = JsonConvert.DeserializeObject<NYTopStoriesModel>(content);

                return $"Hello {System.Text.Json.JsonSerializer.Serialize(model.Articles.Single(a => a.Link.EndsWith(args.shortUrl)), SerializeOptions)}";
            });

            Get("/group/{section}", async args =>
            {
                var res = await nyTimesTopStoriesApiCaller.CallSpecificTopicSectionAsync(args.section);
                res.EnsureSuccessStatusCode();
                var result = await res.Content.ReadAsStringAsync();

                NYTopStoriesModel model = JsonConvert.DeserializeObject<NYTopStoriesModel>(result);
                
                var groupedModel = model.Articles.GroupBy(a => a.Updated.Date).Select(x => new ArticleGroupByDateView
                {
                    Date = x.Key.Date.ToString("yyyy-MM-dd"), Total = x.Count()
                });
                
                return $"Hello {System.Text.Json.JsonSerializer.Serialize(groupedModel, SerializeOptions)}";
            });



            //return $"Hello {System.Text.Json.JsonSerializer.Serialize(model.Articles)}";
        


            //Get("/list/{section}/{updatedDate}", async args =>
            //{
            //    var inputTime = args.section;
            //    DateTime updatedDate;

            //    //if (DateTime.TryParse(args.updatedDate, out updatedDate))
            //    //{

            //    //}

            //    //DateTime.TryParse(args.updatedDate, out updatedDate);


            //    var res = await _nyTimesTopStoriesApiCaller.CallDefaultTopicSectionAsync(args.section);
            //    res.EnsureSuccessStatusCode();
            //    var result = await res.Content.ReadAsStringAsync();

            //    NYTopStoriesModel model = JsonConvert.DeserializeObject<NYTopStoriesModel>(result);

            //    return $"Hello {System.Text.Json.JsonSerializer.Serialize(model.Articles.Where(a => a.Updated == ))}";
            //});


            //Get("/list/{section}", async args =>
            //{
            //    var client = new HttpClient();
            //    client.DefaultRequestHeaders.Add("api-key", "k0XA0k0jJGAVuv8Jr5wAIcKDGPuznmRJ");


            //    var addressNYTimes = "https://api.nytimes.com/svc/topstories/v2/home.json?api-key=k0XA0k0jJGAVuv8Jr5wAIcKDGPuznmRJ";

            //    var basicNYAddress = "https://api.nytimes.com/svc/topstories/v2/home.json";




            //    var nancyAddress = "http://nancyfx.org";
            //    var res = await client.GetAsync(addressNYTimes);
            //    res.EnsureSuccessStatusCode();
            //    var result = await res.Content.ReadAsStringAsync();


            //    ////var parsedObject = JObject.Parse(result);


            //    ////var results = parsedObject["results"].ToString();

            //    //return Response.AsJson(myNewText);
            //    //var projects = JsonConvert.DeserializeObject<List<Models.Project>>(resultString);

            //    ////var model = JsonConvert.DeserializeObject<List<ArticleView>>(results);

            //    var model = JsonConvert.DeserializeObject<NYTopStoriesModel>(result);

            //    return $"Hello {result}";
            //});
        }
    }
}