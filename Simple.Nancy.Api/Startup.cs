using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nancy.Owin;
using Simple.Nancy.Api.Helpers;

namespace Simple.Nancy.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<INyTimesTopStoriesApiCaller, NyTimesTopStoriesApiCaller>( c =>
            {
                c.BaseAddress = new Uri("https://api.nytimes.com/svc/topstories/v2/");
            });

            services.AddTransient<INYTimesTopStoriesHelper, NYTimesTopStoriesHelper>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOwin(x => x.UseNancy());

            app.UseRouting();
        }
    }
}
