using Booking.Models;
using Booking.Services;
using Microsoft.AspNetCore.Identity;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ProductService>();
        services.AddScoped<ProductTypeSevices>();
        services.AddScoped<BranchServices>();
        services.AddScoped<ServiceServices>();
        services.AddScoped<ServicesTypeServices>();
        services.AddScoped<ProviderSerivces>();
        services.AddScoped<LoginService>();
        services.AddScoped<GenerateRandomKey>();
        return services;
    }
}

