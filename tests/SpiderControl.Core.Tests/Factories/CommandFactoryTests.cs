using SpiderControl.Core.Commands;
using SpiderControl.Core.Factories;
using SpiderControl.Core.Interfaces;

namespace SpiderControl.Core.Tests.Factories;

public class CommandFactoryTests
{
    private readonly ICommandFactory _commandFactory;

    public CommandFactoryTests()
    {
        _commandFactory = new CommandFactory();
    }

    [Theory]
    [InlineData('F', typeof(ForwardCommand))]
    [InlineData('L', typeof(RotateLeftCommand))]
    [InlineData('R', typeof(RotateRightCommand))]
    [InlineData('f', typeof(ForwardCommand))]
    [InlineData('l', typeof(RotateLeftCommand))]
    [InlineData('r', typeof(RotateRightCommand))]
    public void CreateCommand_ValidInput_ReturnsCorrectCommand(char input, Type expectedType)
    {
        // Act
        var result = _commandFactory.CreateCommand(input);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.IsType(expectedType, result.Value);
    }

    [Theory]
    [InlineData(']')]
    [InlineData('V')]
    [InlineData('#')]
    public void CreateCommand_InvalidInput_ReturnsFailure(char input)
    {
        // Act
        var result = _commandFactory.CreateCommand(input);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(input.ToString(), result.Error);
    }
}
