using Microsoft.Extensions.Logging;
using Moq;
using SpiderControl.Application.Interfaces;
using SpiderControl.Application.Services;
using SpiderControl.Core.Common;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Application.Tests.Services;

public class WallInputParserTests
{
    private readonly IWallInputParser _wallInputParser;
    private readonly Mock<IValidatorService> _validatorServiceMock;
    private readonly Mock<ILogger<IWallInputParser>> _loggerMock;

    public WallInputParserTests()
    {
        _loggerMock = new Mock<ILogger<IWallInputParser>>();
        _validatorServiceMock = new Mock<IValidatorService>();

        _wallInputParser = new WallInputParser(_validatorServiceMock.Object, _loggerMock.Object);

        _validatorServiceMock.Setup(x => x.ValidateWall(It.IsAny<WallModel>()))
            .Returns(Result<Unit>.Success(Unit.Value));
    }

    [Theory]
    [InlineData("5 10", 5, 10)]
    [InlineData("0 0", 0, 0)]
    [InlineData("1 999", 1, 999)]
    public void ParseWallDimensions_ValidInput_ReturnsSuccessResult(string input, int expectedWidth, int expectedHeight)
    {
        // Act
        var result = _wallInputParser.ParseWallDimensions(input);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedWidth, result.Value.Width);
        Assert.Equal(expectedHeight, result.Value.Height);

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
    public void ParseWallDimensions_InvalidInput_ReturnsFailureResult(string input)
    {
        // Act
        var result = _wallInputParser.ParseWallDimensions(input);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Error);
    }

    [Fact]
    public void ParseWallDimensions_ValidationFails_ReturnsFailureResult()
    {
        // Arrange
        var validationError = "Wall width must be positive";
        _validatorServiceMock.Setup(x => x.ValidateWall(It.IsAny<WallModel>()))
            .Returns(Result<Unit>.Failure(validationError));

        // Act
        var result = _wallInputParser.ParseWallDimensions("5 10");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(validationError, result.Error);
    }
}
