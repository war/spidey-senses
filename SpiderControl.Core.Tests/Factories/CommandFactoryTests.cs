using SpiderControl.Core.Commands;
using SpiderControl.Core.Exceptions;
using SpiderControl.Core.Factories;
using SpiderControl.Core.Interfaces;

namespace SpiderControl.Application.Tests.Factories;

public class CommandFactoryTests
{
    private readonly ICommandFactory _commandFactory;

    public CommandFactoryTests()
    {
        _commandFactory = new CommandFactory();
    }

    [Fact]
    public void CreateForwardCommand_ValidInput_ReturnsCorrectCommand()
    {
        // Act
        var command = _commandFactory.CreateCommand('F');

        // Assert
        Assert.IsType<ForwardCommand>(command);
    }

    [Fact]
    public void CreateRotateLeftCommand_ValidInput_ReturnsCorrectCommand()
    {
        // Act
        var command = _commandFactory.CreateCommand('L');

        // Assert
        Assert.IsType<RotateLeftCommand>(command);
    }

    [Fact]
    public void CreateRotateRightCommand_ValidInput_ReturnsCorrectCommand()
    {
        // Act
        var command = _commandFactory.CreateCommand('R');

        // Assert
        Assert.IsType<RotateRightCommand>(command);
    }

    [Theory]
    [InlineData(']')]
    [InlineData('V')]
    [InlineData('.')]
    [InlineData('#')]
    public void CreateCommand_InvalidInput_ThrowsException(char input)
    {
        // Act
        var commands = () => _commandFactory.CreateCommand(input);

        // Assert
        Assert.Throws<ArgumentException>(commands);
    }
}
