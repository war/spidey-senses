using Microsoft.Extensions.Logging;
using Moq;
using SpiderControl.Core.Commands;
using SpiderControl.Core.Enums;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;
using SpiderControl.Core.Services;

namespace SpiderControl.Core.Tests.Services;

public class SpiderServiceTests
{
    private readonly ISpiderService _spiderService;
    private readonly Mock<ILogger<SpiderService>> _loggerMock;

    public SpiderServiceTests()
    {
        _loggerMock = new Mock<ILogger<SpiderService>>();
        _spiderService = new SpiderService(_loggerMock.Object);
    }

    [Fact]
    public void CreateSpider_ReturnsSpiderWithCorrectPosition()
    {
        // Arrange
        var spider = _spiderService.CreateSpider(3, 5, Orientation.Right);

        // Assert
        Assert.Equal(3, spider.X);
        Assert.Equal(5, spider.Y);
        Assert.Equal(Orientation.Right, spider.Orientation);
    }

    [Fact]
    public void ProcessCommands_ExecutesAllCommands()
    {
        // Arrange
        var spider = new Spider(2, 4, Orientation.Up);
        var wall = new WallModel(5, 8);
        var commands = new List<ICommand>()
        {
            new ForwardCommand(),
            new RotateLeftCommand(),
            new ForwardCommand(),
            new ForwardCommand(),
            new RotateRightCommand(),
        };

        // Act
        _spiderService.ProcessCommands(spider, wall, commands);

        // Assert
        Assert.Equal(0, spider.X);
        Assert.Equal(5, spider.Y);
        Assert.Equal(Orientation.Up, spider.Orientation);
    }
}
