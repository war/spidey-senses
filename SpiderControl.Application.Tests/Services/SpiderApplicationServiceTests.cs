using SpiderControl.Application.Interfaces;
using SpiderControl.Application.Services;
using SpiderControl.Core.Enums;
using SpiderControl.Core.Factories;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;
using SpiderControl.Core.Services;

namespace SpiderControl.Application.Tests.Services;

public class SpiderApplicationServiceTests
{
    private readonly ISpiderApplicationService _spiderApplicationService;

    public SpiderApplicationServiceTests()
    {
        _spiderApplicationService = new SpiderApplicationService();
    }

    [Fact]
    public void ProcessSpiderCommand_ValidInput_ReturnsCorrectOutput()
    {
        // Arrange
        var wallInput = "10 12";
        var spiderInput = "6 8 Right";
        var commandInput = "FLFLFLRRFF";

        var expectedOutput = "6 11 Up";

        // Act
        var result = _spiderApplicationService.ProcessSpiderCommands(wallInput, spiderInput, commandInput);

        //TODO: maybe some other tests like DI "was service created only once" type of thing

        // Assert
        Assert.Equal(expectedOutput, result);
    }

    [Theory]
    [InlineData("7 10", "2 4 Left", "FLFLFRFFLF", "3 1 Right")]
    [InlineData("5 5", "3 3 Up", "FLFLFRFFLFRR", "0 2 Up")]
    [InlineData("3 3", "0 0 Right", "FLFLFRFFLLL", "0 3 Right")]
    public void ProcessSpiderCommand_ValidInputs_ReturnsCorrectOutput(string wallInput, string spiderInput, string commandInput, string expectedOutput)
    {
        // Act
        var result = _spiderApplicationService.ProcessSpiderCommands(wallInput, spiderInput, commandInput);

        // Assert
        Assert.Equal(expectedOutput, result);
    }
}
