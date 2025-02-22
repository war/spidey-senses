using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SpiderControl.Core.Configuration;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;
using SpiderControl.Core.Services;

namespace SpiderControl.Core.Tests.Services;

public class ValidatorServiceTests
{
    private readonly IValidatorService _validatorService;
    private readonly Mock<ILogger<ValidatorService>> _loggerMock;
    private readonly Mock<IOptions<SpiderControlConfig>> _configMock;
    private readonly SpiderControlConfig _defaultConfig;

    public ValidatorServiceTests()
    {
        _loggerMock = new Mock<ILogger<ValidatorService>>();
        _defaultConfig = new SpiderControlConfig { ValidCommands = new[] { 'F', 'L', 'R' } };
        _configMock = new Mock<IOptions<SpiderControlConfig>>();
        _configMock.Setup(x => x.Value).Returns(_defaultConfig);
        _validatorService = new ValidatorService(_loggerMock.Object, _configMock.Object);
    }

    [Fact]
    public void ValidateSpider_ValidInput_ReturnsSuccessResult()
    {
        // Arrange
        var spider = new Spider(5, 5, Enums.Orientation.Up);
        var wall = new WallModel(10, 10);

        // Act
        var resultModel = _validatorService.ValidateSpider(spider);
        var resultPosition = _validatorService.ValidateSpiderPosition(spider, wall);

        var resultBool = resultModel.IsValid && resultPosition.IsValid;
        var resultErrors = resultModel.Errors;
        resultErrors.AddRange(resultPosition.Errors);

        // Assert
        Assert.True(resultBool);
        Assert.Empty(resultErrors);
    }

    [Fact]
    public void ValidateWall_ValidInput_ReturnsSuccessResult()
    {
        // Arrange
        var wall = new WallModel(10, 10);

        // Act
        var result = _validatorService.ValidateWall(wall);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData('F')]
    [InlineData('R')]
    [InlineData('L')]
    public void ValidateCommand_ValidInput_ReturnsSuccessResult(char command)
    {
        // Act
        var result = _validatorService.ValidateCommand(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData('y')]
    [InlineData('5')]
    [InlineData(' ')]
    public void ValidateCommand_InvalidInput_ReturnsFailureResult(char command)
    {
        // Act
        var result = _validatorService.ValidateCommand(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains($"Command must be one of: {string.Join(", ", _defaultConfig.ValidCommands)}",
            result.Errors.First().ErrorMessage);
    }

    [Fact]
    public void ValidateCommands_ValidInput_ReturnsSuccessResult()
    {
        // Arrange
        var commands = new[] { 'F', 'L', 'R', 'F', 'F' };

        // Act
        var result = _validatorService.ValidateCommands(commands);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ValidateCommands_InvalidInput_ReturnsFailureResult()
    {
        // Arrange
        var commands = new[] { 'F', 'X', 'R', '8', 'F', 'L' };

        // Act
        var result = _validatorService.ValidateCommands(commands);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(2, result.Errors.Count);

        foreach (var error in result.Errors)
        {
            Assert.Contains($"Command must be one of: {string.Join(", ", _defaultConfig.ValidCommands)}",
                error.ErrorMessage);
        }
    }

    [Fact]
    public void Constructor_NullLogger_ThrowsArgumentNullException()
    {
        // Act
        var validationService = () => new ValidatorService(null!, _configMock.Object);

        // Assert
        Assert.Throws<ArgumentNullException>(validationService);
    }

    [Fact]
    public void Constructor_NullConfig_ThrowsArgumentNullException()
    {
        // Act
        var validationService = () => new ValidatorService(_loggerMock.Object, null!);

        // Assert
        Assert.Throws<ArgumentNullException>(validationService);
    }

    [Fact]
    public void ValidateCommand_CustomCommands_ValidatesCorrectly()
    {
        // Arrange
        var customConfig = new SpiderControlConfig { ValidCommands = new[] { 'X', 'Y', 'Z' } };
        var customConfigMock = new Mock<IOptions<SpiderControlConfig>>();

        customConfigMock.Setup(x => x.Value).Returns(customConfig);

        var customValidatorService = new ValidatorService(_loggerMock.Object, customConfigMock.Object);

        // Act & Assert
        Assert.True(customValidatorService.ValidateCommand('X').IsValid);
        Assert.True(customValidatorService.ValidateCommand('Y').IsValid);
        Assert.True(customValidatorService.ValidateCommand('Z').IsValid);
        Assert.False(customValidatorService.ValidateCommand('F').IsValid);
    }
}
