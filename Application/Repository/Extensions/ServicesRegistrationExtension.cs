using System;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repository;

namespace Application.Repository.Extensions;

public static class ServicesRegistrationExtension
{
    public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
    {
        services.AddScoped<IQueryProfileRepository, DbRepository>();
        return services;
    }
}
