using HomeManger.Data;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace HomeMangerAPI.Services;


public static class ServicesExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
       // services.AddSingleton(env);
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<CategoryProxy, CategoryProxy>();
        return services;
    }
}
