using Moq;
using SpiderControl.Application.Interfaces;
using SpiderControl.Application.Services;
using SpiderControl.Core.Commands;
using SpiderControl.Core.Common;
using SpiderControl.Core.Interfaces;

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
    }

    [Fact]
    public void ParseCommands_ValidInput_ReturnsSuccessResult()
    {
        // Arrange
        var input = "FRLF";

        _validatorServiceMock.Setup(x => x.ValidateCommands(It.IsAny<IEnumerable<char>>()))
            .Returns(Result<Unit>.Success(Unit.Value));

        _commandFactoryMock.Setup(x => x.CreateCommand('F'))
            .Returns(Result<ICommand>.Success(new ForwardCommand()));
        _commandFactoryMock.Setup(x => x.CreateCommand('R'))
            .Returns(Result<ICommand>.Success(new RotateRightCommand()));
        _commandFactoryMock.Setup(x => x.CreateCommand('L'))
            .Returns(Result<ICommand>.Success(new RotateLeftCommand()));

        // Act
        var result = _commandInputParser.ParseCommands(input);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(4, result.Value.Count());

        _commandFactoryMock.Verify(x => x.CreateCommand('F'), Times.Exactly(2));
        _commandFactoryMock.Verify(x => x.CreateCommand('R'), Times.Once);
        _commandFactoryMock.Verify(x => x.CreateCommand('L'), Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void ParseCommands_EmptyInput_ReturnsFailureResult(string input)
    {
        // Act
        var result = _commandInputParser.ParseCommands(input);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("empty", result.Error);
    }

    [Fact]
    public void ParseCommands_ValidationFails_ReturnsFailureResult()
    {
        // Arrange
        var input = "FRLX";

        _validatorServiceMock.Setup(x => x.ValidateCommands(It.IsAny<IEnumerable<char>>()))
            .Returns(Result<Unit>.Failure("Invalid command found"));

        // Act
        var result = _commandInputParser.ParseCommands(input);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Invalid command found", result.Error);
    }

    [Fact]
    public void ParseCommands_CommandFactoryFails_ReturnsFailureResult()
    {
        // Arrange
        var input = "FRX";

        _validatorServiceMock.Setup(x => x.ValidateCommands(It.IsAny<IEnumerable<char>>()))
            .Returns(Result<Unit>.Success(Unit.Value));

        _commandFactoryMock.Setup(x => x.CreateCommand('F'))
            .Returns(Result<ICommand>.Success(new ForwardCommand()));
        _commandFactoryMock.Setup(x => x.CreateCommand('R'))
            .Returns(Result<ICommand>.Success(new RotateRightCommand()));
        _commandFactoryMock.Setup(x => x.CreateCommand('X'))
            .Returns(Result<ICommand>.Failure("Invalid command: X"));

        // Act
        var result = _commandInputParser.ParseCommands(input);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Invalid command: X", result.Error);
    }
}
