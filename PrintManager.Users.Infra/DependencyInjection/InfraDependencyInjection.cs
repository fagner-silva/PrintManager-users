using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrintManager.Users.Domain.Interfaces;
using PrintManager.Users.Infra.Context;
using PrintManager.Users.Infra.Repositories;
using PrintManager.Users.Infra.Services;
using PrintManager.Users.Infra.Settings;

namespace PrintManager.Users.Infra.DependencyInjection;

public static class InfraDependencyInjection
{
    public static IServiceCollection AddInfra(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(options =>
            configuration.GetSection(MongoDbSettings.SectionName).Bind(options));

        services.Configure<JwtSettings>(options =>
            configuration.GetSection(JwtSettings.SectionName).Bind(options));

        services.AddSingleton<MongoContext>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasherService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ICompanyMemberRepository, CompanyMemberRepository>();

        return services;
    }
}