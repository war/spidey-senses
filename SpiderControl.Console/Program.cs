using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SpiderControl.Application;
using SpiderControl.Application.Interfaces;
using SpiderControl.Console.IO;
using SysConsole = System.Console;

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

        SysConsole.WriteLine($"Final Output: {result}.");
        SysConsole.WriteLine("Press any key to continue...");
        SysConsole.ReadLine();
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

