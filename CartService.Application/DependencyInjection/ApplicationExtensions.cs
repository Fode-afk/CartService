using CartService.Application.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using migApp.Shared.Domain.Primitives;

namespace CartService.Application.DependencyInjection;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services
            .AddValidators()
            .AddMediatR()
            .AddDomainEventHandlers()
            .AddTimeProvider();

    private static IServiceCollection AddValidators(this IServiceCollection services) =>
        services.AddValidatorsFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);

    private static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);
        });

        return services;
    }

    private static IServiceCollection AddDomainEventHandlers(this IServiceCollection services)
    {
        var assembly = typeof(ApplicationAssemblyMarker).Assembly;

        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(IPostCommitDomainEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }

    private static IServiceCollection AddTimeProvider(this IServiceCollection services) =>
        services.AddSingleton(TimeProvider.System);
}
