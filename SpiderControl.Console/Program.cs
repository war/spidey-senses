using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SpiderControl.Application;
using SpiderControl.Application.Interfaces;
using SpiderControl.Console.IO;

namespace SpiderControl.Console;

public class Program
{
    public static void Main(String[] args)
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        
        var serviceProvider = services.BuildServiceProvider();
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        var spiderApplicationService = serviceProvider.GetRequiredService<ISpiderApplicationService>();

        var inputReader = new ConsoleInputReader();

        var inputs = inputReader.ReadInputs();
        var result = spiderApplicationService.ProcessSpiderCommands(
            inputs.SpiderPosition,
            inputs.WallDimensions, 
            inputs.Commands
        );

        System.Console.WriteLine($"Final Output: {result}.");
        System.Console.WriteLine("Press any key to continue...");
        System.Console.ReadLine();
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(builder =>
        {
            builder.Services.AddLogging();
            builder.SetMinimumLevel(LogLevel.Information);
        });

        services.AddSpiderControlServices();
    }
}

