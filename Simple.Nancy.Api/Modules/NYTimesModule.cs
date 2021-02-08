using Nancy;
using Simple.Nancy.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Simple.Nancy.Api.Models.ViewModels;

namespace Simple.Nancy.Api.Modules
{
    public sealed class NYTimesModule : NancyModule
    {
        private static readonly JsonSerializerOptions SerializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public NYTimesModule(INyTimesTopStoriesApiCaller nyTimesTopStoriesApiCaller, 
            INYTimesTopStoriesHelper nyTimesTopStoriesHelper)
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
                var articles = await nyTimesTopStoriesHelper.GetSpecificSectionArticles(args.section);
                return JsonSerializer.Serialize(articles, SerializeOptions);
            });

            Get("/list/{section}/first", async args =>
            {
                var articles = await nyTimesTopStoriesHelper.GetSpecificSectionArticles(args.section);

                return $"{JsonSerializer.Serialize(articles.First(), SerializeOptions)}";
            });


            Get("/list/{section}/{updatedDate}", async args =>
            {
                if (!DateTime.TryParse(args.updatedDate, out DateTime updatedDate))
                {
                    throw new FormatException("Wrong date format, advised format is yyyy-MM-dd, example: 1992-12-31");
                }

                IList<ArticleView> articles = await nyTimesTopStoriesHelper.GetSpecificSectionArticles(args.section);

                return $"{JsonSerializer.Serialize(articles.Where(a => a.Updated.Date == updatedDate.Date), SerializeOptions)}";
            });

            Get("/article/{shortUrl}", async args =>
            {
                var articles = await nyTimesTopStoriesHelper.GetDefaultSectionArticles();

                return $"{JsonSerializer.Serialize(articles.Single(a => a.Link.EndsWith(args.shortUrl)), SerializeOptions)}";
            });

            Get("/group/{section}", async args =>
            {
                IList<ArticleView> articles = await nyTimesTopStoriesHelper.GetSpecificSectionArticles(args.section);

                var groupedModel = articles.GroupBy(a => a.Updated.Date).Select(x => new ArticleGroupByDateView
                {
                    Date = x.Key.Date.ToString("yyyy-MM-dd"),
                    Total = x.Count()
                });

                return $"{JsonSerializer.Serialize(groupedModel, SerializeOptions)}";
            });
        }
    }
}