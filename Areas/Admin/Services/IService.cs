using Microsoft.Extensions.DependencyInjection;
using Booking.Services;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ProductService>();
        services.AddScoped<ProductTypeSevices>();
        services.AddScoped<BranchServices>();
        services.AddScoped<ServiceServices>();

        return services;
    }
}

