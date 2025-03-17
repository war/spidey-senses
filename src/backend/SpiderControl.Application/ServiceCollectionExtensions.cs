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
        services.AddSingleton<IWallInputParser, WallInputParser>();
        services.AddSingleton<ISpiderApplicationService, SpiderApplicationService>();

        services.AddSingleton<IValidatorService, ValidatorService>();

        services.AddSingleton<ISpiderInputParser, SpiderInputParser>();
        services.AddSingleton<IWallInputParser, WallInputParser>();
        services.AddSingleton<ICommandInputParser, CommandInputParser>();

        return services;
    }
}
