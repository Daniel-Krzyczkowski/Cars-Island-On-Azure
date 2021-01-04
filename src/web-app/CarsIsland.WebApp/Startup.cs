using CarsIsland.WebApp.Services;
using CarsIsland.WebApp.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace CarsIsland.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();

            services.AddMicrosoftIdentityWebAppAuthentication(Configuration, "AzureAdB2C")
                    .EnableTokenAcquisitionToCallDownstreamApi(new string[] { Configuration["CarsIslandApi:Scope"] })
                    .AddInMemoryTokenCaches();

            services.AddControllersWithViews()
                    .AddMicrosoftIdentityUI();

            services.AddRazorPages();
            services.AddServerSideBlazor().AddMicrosoftIdentityConsentHandler();

            services.AddHttpClient<ICarsIslandApiService, CarsIslandApiService>(configureClient =>
            {
                configureClient.BaseAddress = new Uri(Configuration.GetSection("CarsIslandApi:Url").Value);
            })
            .AddPolicyHandler(GetRetryPolicy(services))
            .AddPolicyHandler(GetCircuitBreakerPolicy(services));


            services.AddHttpContextAccessor();
            services.AddTransient<IIdentityService, IdentityService>();
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(IServiceCollection services)
        {
            return HttpPolicyExtensions
              // Handle HttpRequestExceptions, 408 and 5xx status codes:
              .HandleTransientHttpError()
              // Handle 404 not found
              .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
              // Retry 3 times, each time wait 2, 4 and 8 seconds before retrying:
              .WaitAndRetryAsync(new[]
                {
                   TimeSpan.FromSeconds(2),
                   TimeSpan.FromSeconds(4),
                   TimeSpan.FromSeconds(8)
                },
                 onRetry: (outcome, timespan, retryAttempt, context) =>
                 {
                     services.BuildServiceProvider()
                             .GetRequiredService<ILogger<CarsIslandApiService>>()?
                             .LogError("Delaying for {delay}ms, then making retry: {retry}.", timespan.TotalMilliseconds, retryAttempt);
                 });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(IServiceCollection services)
        {
            return HttpPolicyExtensions
              // Handle HttpRequestExceptions, 408 and 5xx status codes:
              .HandleTransientHttpError()
              .CircuitBreakerAsync(3, TimeSpan.FromSeconds(10),
              onBreak: (result, timeSpan, context) =>
               {
                   services.BuildServiceProvider()
                               .GetRequiredService<ILogger<CarsIslandApiService>>()?
                               .LogError("CircuitBreaker onBreak for {delay}ms", timeSpan.TotalMilliseconds);
               },
              onReset: context =>
              {
                  services.BuildServiceProvider()
                               .GetRequiredService<ILogger<CarsIslandApiService>>()?
                               .LogError("CircuitBreaker closed again");
              },
              onHalfOpen: () =>
              {
                  services.BuildServiceProvider()
                              .GetRequiredService<ILogger<CarsIslandApiService>>()?
                              .LogError("CircuitBreaker onHalfOpen");
              });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
