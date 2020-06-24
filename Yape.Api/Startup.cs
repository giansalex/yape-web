using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Polly;
using Refit;
using Yape.Api.Models;
using Yape.Api.Repository;
using Yape.Api.Services;
using Yape.Sdk;

namespace Yape.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.Configure<YapeSettings>(Configuration.GetSection("Yape"));
            services.AddTransient<IPinResolver, PinEncrypt>();
            services.AddTransient<ITokenStore, CacheTokenStore>();
            services.AddTransient<IOrderRepository, CacheOrderRepository>();
            services.AddTransient<OrderGenerator>();
            services.AddTransient<YapeClient>();
            services.AddTransient<IYapeClient, YapeAuthClient>(s =>
            {
                var settings = s.GetService<IOptions<YapeSettings>>().Value;
                return new YapeAuthClient(
                    s.GetService<YapeClient>(),
                    s.GetService<IYapeApi>(),
                    s.GetService<IPinResolver>(),
                    s.GetService<ITokenStore>())
                {
                    UserId = settings.UserId,
                    Email = settings.Email,
                    Pin = settings.Pin
                };
            });
            services.AddTransient<SecurityHttpClientHandler>();
            services.AddRefitClient<IYapeApi>(new RefitSettings(new SystemTextJsonContentSerializer()))
                .ConfigureHttpClient((s, c) => c.BaseAddress = new Uri(s.GetService<IOptions<YapeSettings>>().Value.Endpoint))
                .AddHttpMessageHandler<SecurityHttpClientHandler>()
                .AddTransientHttpErrorPolicy(p => 
                    p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600)));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration["Redis:Connection"];
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("API ready!");
                });
                endpoints.MapControllers();
            });
        }
    }
}
