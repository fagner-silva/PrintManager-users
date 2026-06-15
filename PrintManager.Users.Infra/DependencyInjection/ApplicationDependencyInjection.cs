using Microsoft.Extensions.DependencyInjection;
using PrintManager.Users.Application.Interfaces;
using PrintManager.Users.Application.Mappers;
using PrintManager.Users.Application.Services;
using PrintManager.Users.Application.Validators;

namespace PrintManager.Users.Application.DependencyInjection;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserMapper, UserMapper>();
        services.AddScoped<IUserValidator, UserValidator>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<ICompanyMemberService, CompanyMemberService>();

        return services;
    }
}