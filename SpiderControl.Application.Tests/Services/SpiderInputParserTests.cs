using Microsoft.Extensions.Logging;
using Moq;
using SpiderControl.Application.Interfaces;
using SpiderControl.Application.Services;
using SpiderControl.Core.Common;
using SpiderControl.Core.Enums;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Application.Tests.Services;

public class SpiderInputParserTests
{
    private readonly Mock<IValidatorService> _validatorServiceMock;
    private readonly ISpiderInputParser _spiderInputParser;
    private readonly Mock<ILogger<ISpiderInputParser>> _loggerMock;

    public SpiderInputParserTests()
    {
        _validatorServiceMock = new Mock<IValidatorService>();
        _loggerMock = new Mock<ILogger<ISpiderInputParser>>();
        _spiderInputParser = new SpiderInputParser(_validatorServiceMock.Object, _loggerMock.Object);
    }

    [Theory]
    [InlineData("5 9 Up", 5, 9, Orientation.Up)]
    [InlineData("1 2 Right", 1, 2, Orientation.Right)]
    public void ParseSpiderPosition_ValidInput_ReturnsSuccessResult(string input, int expectedX, int expectedY, Orientation expectedOrientation)
    {
        // Arrange
        _validatorServiceMock.Setup(x => x.ValidateSpider(It.IsAny<Spider>()))
            .Returns(Result<Unit>.Success(Unit.Value));

        // Act
        var result = _spiderInputParser.ParseSpiderPosition(input);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedX, result.Value.X);
        Assert.Equal(expectedY, result.Value.Y);
        Assert.Equal(expectedOrientation, result.Value.Orientation);
        _validatorServiceMock.Verify(x => x.ValidateSpider(
            It.Is<Spider>(s => s.X == expectedX && s.Y == expectedY && s.Orientation == expectedOrientation)),
            Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData("1")]
    [InlineData("1 2")]
    [InlineData("1 9 15")]
    [InlineData("1 2 Invalid")]
    public void ParseSpiderPosition_InvalidInput_ReturnsFailureResult(string input)
    {
        // Act
        var result = _spiderInputParser.ParseSpiderPosition(input);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Invalid format", result.Error);
    }

    [Fact]
    public void ParseSpiderPosition_ValidationFails_ReturnsFailureResult()
    {
        // Arrange
        var validationError = "X position is invalid";
        _validatorServiceMock.Setup(x => x.ValidateSpider(It.IsAny<Spider>()))
            .Returns(Result<Unit>.Failure(validationError));

        // Act
        var result = _spiderInputParser.ParseSpiderPosition("5 5 Up");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(validationError, result.Error);
    }
}
