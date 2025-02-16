using FluentValidation.Results;
using Moq;
using SpiderControl.Application.Interfaces;
using SpiderControl.Application.Services;
using SpiderControl.Core.Enums;
using SpiderControl.Core.Exceptions;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Application.Tests.Services;

public class SpiderInputParserTests
{
    private readonly Mock<IValidatorService> _validationServiceMock;
    private readonly ISpiderInputParser _spiderInputParser;

    public SpiderInputParserTests()
    {
        _validationServiceMock = new Mock<IValidatorService>();
        _spiderInputParser = new SpiderInputParser(_validationServiceMock.Object);
    }

    [Theory]
    [InlineData("5 9 Up", 5, 9, Orientation.Up)]
    [InlineData("1 2 Right", 1, 2, Orientation.Right)]

    public void ParseSpiderPosition_ValidInput_ReturnsSpiderModel(string input, int expectedX, int expectedY, Orientation expectedOrientation)
    {
        // Act
        _validationServiceMock.Setup(x => x.ValidateSpider(It.IsAny<Spider>()))
            .Returns(new ValidationResult());
        _validationServiceMock.Setup(x => x.ValidateWall(It.IsAny<WallModel>()))
            .Returns(new ValidationResult());
        _validationServiceMock.Setup(x => x.ValidateCommands(It.IsAny<IEnumerable<char>>()))
            .Returns(new ValidationResult());

        var spider = _spiderInputParser.ParseSpiderPosition(input);

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
        var spider = () => _spiderInputParser.ParseSpiderPosition(input);

        // Assert
        Assert.Throws<InputParseException>(spider);
    }
}
