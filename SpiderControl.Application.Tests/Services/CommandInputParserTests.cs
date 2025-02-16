using FluentValidation.Results;
using Moq;
using SpiderControl.Application.Interfaces;
using SpiderControl.Application.Services;
using SpiderControl.Core.Commands;
using SpiderControl.Core.Exceptions;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Application.Tests.Services;

public class CommandInputParserTests
{
    private readonly ICommandInputParser _commandInputParser;
    private readonly Mock<ICommandFactory> _commandFactoryMock;
    private readonly Mock<IValidatorService> _validatorServiceMock;

    public CommandInputParserTests()
    {
        _commandFactoryMock = new Mock<ICommandFactory>();
        _validatorServiceMock = new Mock<IValidatorService>();

        _commandInputParser = new CommandInputParser(_commandFactoryMock.Object, _validatorServiceMock.Object);

        _validatorServiceMock.Setup(x => x.ValidateSpider(It.IsAny<Spider>()))
            .Returns(new ValidationResult());
        _validatorServiceMock.Setup(x => x.ValidateWall(It.IsAny<WallModel>()))
            .Returns(new ValidationResult());
        _validatorServiceMock.Setup(x => x.ValidateCommands(It.IsAny<IEnumerable<char>>()))
            .Returns(new ValidationResult());
    }

    [Fact]
    public void ParseCommands_ValidInput_ReturnsCommandList()
    {
        // Arrange
        _commandFactoryMock.Setup(x => x.CreateCommand('F'))
            .Returns(new ForwardCommand());
        _commandFactoryMock.Setup(x => x.CreateCommand('R'))
            .Returns(new RotateRightCommand());
        _commandFactoryMock.Setup(x => x.CreateCommand('L'))
            .Returns(new RotateLeftCommand());

        var commands = _commandInputParser.ParseCommands("FRLF").ToList();

        // Assert
        Assert.Equal(4, commands.Count);
        Assert.IsType<ForwardCommand>(commands[0]);
        Assert.IsType<RotateRightCommand>(commands[1]);
        Assert.IsType<RotateLeftCommand>(commands[2]);
        Assert.IsType<ForwardCommand>(commands[3]);

        _commandFactoryMock.Verify(x => x.CreateCommand('F'), Times.Exactly(2));
        _commandFactoryMock.Verify(x => x.CreateCommand('R'), Times.Once());
        _commandFactoryMock.Verify(x => x.CreateCommand('L'), Times.Once());
    }

    [Theory]
    [InlineData("")]
    public void ParseCommands_InvalidInput_ThrowsException(string input)
    {
        // Arrange
        var commands = () => _commandInputParser.ParseCommands(input);

        // Assert
        Assert.Throws<InputParseException>(commands);
    }
}
