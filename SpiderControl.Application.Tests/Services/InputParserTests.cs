using FluentValidation.Results;
using Moq;
using SpiderControl.Application.Interfaces;
using SpiderControl.Application.Services;
using SpiderControl.Core.Commands;
using SpiderControl.Core.Enums;
using SpiderControl.Core.Exceptions;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Application.Tests.Services;

public class InputParserTests
{
    private readonly IInputParser _inputParser;
    private readonly Mock<ICommandFactory> _commandFactoryMock;
    private readonly Mock<IValidatorService> _validatorServiceMock;

    public InputParserTests()
    {
        _commandFactoryMock = new Mock<ICommandFactory>();
        _validatorServiceMock = new Mock<IValidatorService>();

        _inputParser = new InputParser(_commandFactoryMock.Object, _validatorServiceMock.Object);
        
        _validatorServiceMock.Setup(x => x.ValidateSpider(It.IsAny<SpiderModel>()))
            .Returns(new ValidationResult());
        _validatorServiceMock.Setup(x => x.ValidateWall(It.IsAny<WallModel>()))
            .Returns(new ValidationResult());
        _validatorServiceMock.Setup(x => x.ValidateCommands(It.IsAny<IEnumerable<char>>()))
            .Returns(new ValidationResult());
    }

    [Theory]
    [InlineData("5 10", 5, 10)]
    [InlineData("0 0", 0, 0)]
    [InlineData("1 999", 1, 999)]
    public void ParseWallDimensions_ValidInput_ReturnsWallModel(string input, int expectedWidth, int expectedHeight)
    {
        // Act
        var wall = _inputParser.ParseWallDimensions(input);

        // Assert
        Assert.Equal(expectedWidth, wall.Width);
        Assert.Equal(expectedHeight, wall.Height);

        _validatorServiceMock.Verify(x => x.ValidateWall(
            It.Is<WallModel>(w => w.Width == expectedWidth && w.Height == expectedHeight)), 
            Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData("1")]
    [InlineData("1 9 15")]
    [InlineData("ab")]
    [InlineData("a b")]
    public void ParseWallDimensions_InvalidInput_ThrowsException(string input)
    {
        // Arrange
        var wall = () => _inputParser.ParseWallDimensions(input);

        // Assert
        Assert.Throws<InputParseException>(wall);
    }

    [Theory]
    [InlineData("5 9 Up", 5, 9, Orientation.Up)]
    [InlineData("1 2 Right", 1, 2, Orientation.Right)]

    public void ParseSpiderPosition_ValidInput_ReturnsSpiderModel(string input, int expectedX, int expectedY, Orientation expectedOrientation)
    {
        // Arrange
        var spider = _inputParser.ParseSpiderPosition(input);

        // Assert
        Assert.Equal(expectedX, spider.X);
        Assert.Equal(expectedY, spider.Y);
        Assert.Equal(expectedOrientation, spider.Orientation);
    }

    [Theory]
    [InlineData("")]
    [InlineData("1")]
    [InlineData("1 2")]
    [InlineData("1 9 15")]
    [InlineData("1 2 Invalid")]
    public void ParseSpiderPosition_InvalidInput_ThrowsException(string input)
    {
        // Arrange
        var spider = () => _inputParser.ParseSpiderPosition(input);

        // Assert
        Assert.Throws<InputParseException>(spider);
    }

    [Fact]
    public void ParseCommands_ValidInput_ReturnsCommandList()
    {
        // Arrange
        var input = "FRLF";

        _commandFactoryMock.Setup(x => x.CreateCommand('F')).Returns(new ForwardCommand());
        _commandFactoryMock.Setup(x => x.CreateCommand('R')).Returns(new RotateRightCommand());
        _commandFactoryMock.Setup(x => x.CreateCommand('L')).Returns(new RotateLeftCommand());

        // Act
        var commands = _inputParser.ParseCommands(input);
        var commandList = commands.ToList();

        // Assert
        Assert.Equal(4, commandList.Count);
        Assert.IsType<ForwardCommand>(commandList[0]);
        Assert.IsType<RotateRightCommand>(commandList[1]);
        Assert.IsType<RotateLeftCommand>(commandList[2]);
        Assert.IsType<ForwardCommand>(commandList[3]);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void ParseCommands_InvalidInput_ThrowsException(string input)
    {
        // Arrange
        var commands = () => _inputParser.ParseCommands(input);

        // Assert
        Assert.Throws<InputParseException>(commands);
    }
}
