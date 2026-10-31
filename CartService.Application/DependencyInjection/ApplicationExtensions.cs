using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using migApp.Shared.Domain.Primitives;

namespace CartService.Application.DependencyInjection;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services
            .AddValidatorsFromAssembly(typeof(ApplicationAssemblyMarker).Assembly)
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly))
            .AddDomainEventHandlers()
            .AddSingleton(TimeProvider.System);

    private static IServiceCollection AddDomainEventHandlers(this IServiceCollection services) =>
        services.Scan(scan => scan
            .FromAssemblies(typeof(ApplicationAssemblyMarker).Assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(IPostCommitDomainEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
}