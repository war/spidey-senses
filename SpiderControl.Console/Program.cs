using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using SpiderControl.Application;
using SpiderControl.Application.Interfaces;
using SpiderControl.Application.Models;
using SpiderControl.Console.IO;

using SysConsole = System.Console;
using SpiderControl.Console.Interfaces;

namespace SpiderControl.Console;

public class Program
{
    public static void Main(String[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var serviceProvider = ConfigureServices(configuration);

        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

        var spiderApplicationService = serviceProvider.GetRequiredService<ISpiderApplicationService>();

        var inputReader = serviceProvider.GetRequiredService<IConsoleInputReader>();
        var inputs = inputReader.ReadInputs();

        var processCommandModel = new ProcessCommandModel
        {
            SpiderInput = inputs.SpiderPosition,
            WallInput = inputs.WallDimensions,
            CommandInput = inputs.Commands
        };

        var result = spiderApplicationService.ProcessSpiderCommands(processCommandModel);

        SysConsole.WriteLine($"Final Output: {result}.");
        SysConsole.WriteLine("Press any key to continue...");
        SysConsole.ReadKey();
    }

    public static ServiceProvider ConfigureServices(IConfiguration configuration)
    {
        var services = new ServiceCollection();

        services.AddLogging(builder =>
        {
            builder
                .SetMinimumLevel(LogLevel.Information)
                .AddConsole();

            builder.AddConfiguration(configuration.GetSection("Logging"));
        });

        services.AddSingleton<IConsoleInputReader, ConsoleInputReader>();

        services.AddSpiderControlServices();

        return services.BuildServiceProvider();
    }
}
