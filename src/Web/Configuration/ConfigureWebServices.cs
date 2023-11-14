using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Web.Configuration;

public static class ConfigureWebServices
{
    public static IServiceCollection AddWebServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddTransient<CustomEmailConfirmationTokenProvider<ApplicationUser>>();

        return services;
    }
}
