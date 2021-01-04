using CarsIsland.API.AuthorizationPolicies;
using CarsIsland.API.Core.DependencyInjection;
using CarsIsland.API.DtoValidation;
using CarsIsland.API.DtoValidation.Filters;
using CarsIsland.API.Middleware;
using CarsIsland.Core.Interfaces;
using CarsIsland.Infrastructure.Services.Identity;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarsIsland.API
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
            services.AddAppConfiguration(Configuration)
                    .AddAuthenticationWithAuthorizationSupport(Configuration)
                    .AddDataServices()
                    .AddStorageServices()
                    .AddMessagingServices()
                    .AddSwagger();

            services.AddControllers(configure =>
            {
                configure.Filters.Add(new DtoValidationFilter());
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                configure.Filters.Add(new AuthorizeFilter(policy));
            }).AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CustomerCarReservationValidator>());

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AccessAsUser",
                        policy => policy.Requirements.Add(new ScopesRequirement("access_as_user")));
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddHttpContextAccessor();
            services.AddTransient<IIdentityService, IdentityService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseApiExceptionHandler();

            app.UseHttpsRedirection();

            app.UseSwaggerServices();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
