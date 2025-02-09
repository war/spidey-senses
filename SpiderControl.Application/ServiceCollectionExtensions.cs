using Microsoft.Extensions.DependencyInjection;
using SpiderControl.Application.Interfaces;
using SpiderControl.Application.Services;
using SpiderControl.Core.Factories;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Services;

namespace SpiderControl.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSpiderControlServices(this IServiceCollection services)
    {
        services.AddSingleton<ISpiderService, SpiderService>();
        services.AddSingleton<ICommandFactory, CommandFactory>();
        services.AddSingleton<IInputParser, InputParser>();
        services.AddSingleton<ISpiderApplicationService, SpiderApplicationService>();

        return services;
    }
}
