using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using TimeOnion.Domain;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.UserManagement.Core;

namespace TimeOnion.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddScoped(typeof(IRepository<,>), typeof(Repository<,>))
            .AddScoped<IClock, SystemClock>()
            .AddSingleton<IReadModelDatabase, InMemoryReadModelDatabase>()
            .AddTransient<IUserScopedReadModelDatabase, ScopeToUserReadModelDatabase>()
            .AddSingleton<DomainEventsCache>()
            .AddScoped<IEventStore, S3StorageEventStore>()
            .Decorate<IEventStore, CachedEventStore>()
            .AddScoped<MinioClient>(x =>
            {
                var configuration = x.GetRequiredService<IOptions<S3StorageConfiguration>>().Value;

                return new MinioClient()
                    .InitializeFrom(configuration)
                    .Build();
            });

        services
            .AddOptions<S3StorageConfiguration>()
            .BindConfiguration(S3StorageConfiguration.SectionName)
            .ValidateDataAnnotations();

        services.AddScoped<IPasswordHasher, AspNetCoreIdentityPasswordHasher>();

        return services;
    }
}