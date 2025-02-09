using Microsoft.Extensions.DependencyInjection;
using SpiderControl.Application;
using SpiderControl.Application.Interfaces;
using SpiderControl.Console.IO;

namespace SpiderControl.Console;

public class Progam
{
    public static void Main(String[] args)
    {
        var services = new ServiceCollection();
        ConfigureServices(services);

        var serviceProvider = services.BuildServiceProvider();

        var spiderApplicationService = serviceProvider.GetRequiredService<ISpiderApplicationService>();

        var inputReader = new ConsoleInputReader();

        var inputs = inputReader.ReadInputs();
        var result = spiderApplicationService.ProcessSpiderCommands(
            inputs.WallDimensions, 
            inputs.SpiderPosition, 
            inputs.Commands
        );

        System.Console.WriteLine($"Final Output: {result}.");
        System.Console.WriteLine("Press any key to continue...");
        System.Console.ReadLine();
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddSpiderControlServices();
    }
}

