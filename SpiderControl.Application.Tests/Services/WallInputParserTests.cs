using FluentValidation.Results;
using Moq;
using SpiderControl.Application.Interfaces;
using SpiderControl.Application.Services;
using SpiderControl.Core.Exceptions;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Application.Tests.Services;

public class WallInputParserTests
{
    private readonly IWallInputParser _wallInputParser;
    private readonly Mock<IValidatorService> _validatorServiceMock;

    public WallInputParserTests()
    {
        _validatorServiceMock = new Mock<IValidatorService>();

        _wallInputParser = new WallInputParser(_validatorServiceMock.Object);

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
        var wall = _wallInputParser.ParseWallDimensions(input);

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
        var wall = () => _wallInputParser.ParseWallDimensions(input);

        // Assert
        Assert.Throws<InputParseException>(wall);
    }
}
