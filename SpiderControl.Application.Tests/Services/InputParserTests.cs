using SpiderControl.Application.Interfaces;
using SpiderControl.Application.Services;
using SpiderControl.Core.Commands;
using SpiderControl.Core.Enums;

namespace SpiderControl.Application.Tests.Services;

public class InputParserTests
{
    private readonly IInputParser _inputParser;

    public InputParserTests()
    {
        _inputParser = new InputParser();
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
        Assert.Throws<ArgumentException>(wall);
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
        Assert.Throws<ArgumentException>(spider);
    }

    [Fact]
    public void ParseCommands_ValidInput_ReturnsCommandList()
    {
        // Arrange
        var commands = _inputParser.ParseCommands("FRLF").ToList();

        // Assert
        Assert.Equal(4, commands.Count);
        Assert.IsType<ForwardCommand>(commands[0]);
        Assert.IsType<RotateRightCommand>(commands[1]);
        Assert.IsType<RotateLeftCommand>(commands[2]);
        Assert.IsType<ForwardCommand>(commands[3]);
    }

    [Theory]
    [InlineData("")]
    public void ParseCommands_InvalidInput_ThrowsException(string input)
    {
        // Arrange
        var commands = () => _inputParser.ParseCommands(input);

        // Assert
        Assert.Throws<ArgumentException>(commands);
    }
}
