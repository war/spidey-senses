using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SpiderControl.Core.Configuration;
using SpiderControl.Core.Enums;
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

    [Theory]
    [InlineData(5, 5, Orientation.Up)]
    [InlineData(0, 0, Orientation.Right)]
    [InlineData(10, 10, Orientation.Down)]
    [InlineData(3, 7, Orientation.Left)]
    public void ValidateSpider_ValidInput_ReturnsSuccess(int x, int y, Orientation orientation)
    {
        // Arrange
        var spider = new Spider(x, y, orientation);

        // Act
        var result = _validatorService.ValidateSpider(spider);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(-1, 5, Orientation.Up)]
    [InlineData(5, -1, Orientation.Right)]
    public void ValidateSpider_InvalidInput_ReturnsFailure(int x, int y, Orientation orientation)
    {
        // Arrange
        var spider = new Spider(x, y, orientation);

        // Act
        var result = _validatorService.ValidateSpider(spider);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("must be greater than or equal to 0", result.Error);
    }

    [Fact]
    public void ValidateWall_ValidInput_ReturnsSuccess()
    {
        // Arrange
        var wall = new WallModel(10, 10);

        // Act
        var result = _validatorService.ValidateWall(wall);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(-1, 10)]
    [InlineData(10, -1)]
    public void ValidateWall_InvalidInput_ReturnsFailure(int width, int height)
    {
        // Arrange
        var wall = new WallModel(width, height);

        // Act
        var result = _validatorService.ValidateWall(wall);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("must be greater than or equal to 0", result.Error);
    }

    [Theory]
    [InlineData(3, 3, 5, 5, Orientation.Up)]
    [InlineData(0, 0, 5, 5, Orientation.Right)]
    [InlineData(5, 5, 5, 5, Orientation.Down)]
    public void ValidateSpiderPosition_ValidInput_ReturnsSuccess(
        int spiderX, int spiderY, int wallWidth, int wallHeight, Orientation orientation)
    {
        // Arrange
        var spider = new Spider(spiderX, spiderY, orientation);
        var wall = new WallModel(wallWidth, wallHeight);

        // Act
        var result = _validatorService.ValidateSpiderPosition(spider, wall);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(6, 3, 5, 5, Orientation.Up)]
    [InlineData(3, 6, 5, 5, Orientation.Right)]
    public void ValidateSpiderPosition_InvalidInput_ReturnsFailure(
        int spiderX, int spiderY, int wallWidth, int wallHeight, Orientation orientation)
    {
        // Arrange
        var spider = new Spider(spiderX, spiderY, orientation);
        var wall = new WallModel(wallWidth, wallHeight);

        // Act
        var result = _validatorService.ValidateSpiderPosition(spider, wall);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("must be less than or equal to", result.Error);
    }

    [Theory]
    [InlineData('F')]
    [InlineData('L')]
    [InlineData('R')]
    [InlineData('f')]
    [InlineData('l')]
    [InlineData('r')]
    public void ValidateCommand_ValidInput_ReturnsSuccess(char command)
    {
        // Act
        var result = _validatorService.ValidateCommand(command);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData('X')]
    [InlineData('5')]
    [InlineData(' ')]
    public void ValidateCommand_InvalidInput_ReturnsFailure(char command)
    {
        // Act
        var result = _validatorService.ValidateCommand(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains($"Command must be one of: {string.Join(", ", _defaultConfig.ValidCommands)}",
            result.Error);
    }

    [Fact]
    public void ValidateCommands_ValidInput_ReturnsSuccess()
    {
        // Arrange
        var commands = new[] { 'F', 'L', 'R', 'F', 'F' };

        // Act
        var result = _validatorService.ValidateCommands(commands);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void ValidateCommands_InvalidInput_ReturnsFailure()
    {
        // Arrange
        var commands = new[] { 'F', 'X', 'R', '8', 'F' };

        // Act
        var result = _validatorService.ValidateCommands(commands);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains($"Command must be one of: {string.Join(", ", _defaultConfig.ValidCommands)}",
            result.Error);
    }

    [Fact]
    public void ValidateCommands_CustomCommands_ValidatesCorrectly()
    {
        // Arrange
        var customConfig = new SpiderControlConfig { ValidCommands = new[] { 'X', 'Y', 'Z' } };
        var customConfigMock = new Mock<IOptions<SpiderControlConfig>>();
        
        customConfigMock.Setup(x => x.Value).Returns(customConfig);
        
        var customValidatorService = new ValidatorService(_loggerMock.Object, customConfigMock.Object);

        // Act & Assert
        Assert.True(customValidatorService.ValidateCommand('X').IsSuccess);
        Assert.True(customValidatorService.ValidateCommand('Y').IsSuccess);
        Assert.True(customValidatorService.ValidateCommand('Z').IsSuccess);
        Assert.False(customValidatorService.ValidateCommand('F').IsSuccess);
    }
}
