using Microsoft.AspNetCore.Builder;

namespace CarsIsland.API.Core.DependencyInjection
{
    public static class AppBuilderCollectionExtensions
    {
        public static void UseSwaggerServices(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cars Island API - v1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
