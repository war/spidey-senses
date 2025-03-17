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

    [Fact]
    public void ProcessCommands_ExecuteNoCommands()
    {
        // Arrange
        var spider = new Spider(2, 4, Orientation.Up);
        var wall = new WallModel(5, 8);
        var commands = new List<ICommand>() {};

        // Act
        var resultSpider = _spiderService.ProcessCommands(spider, wall, commands);

        // Assert
        Assert.True(resultSpider.IsSuccess);
        Assert.Equal(spider.X, resultSpider.Value.X);
        Assert.Equal(spider.Y, resultSpider.Value.Y);
        Assert.Equal(spider.Orientation, resultSpider.Value.Orientation);
    }

    [Fact]
    public void ProcessCommandsWithHistory_ExecutesAllCommands()
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
        var result = _spiderService.ProcessCommandsWithHistory(spider, wall, commands);
        var spiderHistory = result.Value.ToList();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(6, spiderHistory.Count);

        // Verify initial position
        Assert.Equal(2, spiderHistory[0].X);
        Assert.Equal(4, spiderHistory[0].Y);
        Assert.Equal(Orientation.Up, spiderHistory[0].Orientation);

        // Verify final position
        Assert.Equal(0, spiderHistory[5].X);
        Assert.Equal(5, spiderHistory[5].Y);
        Assert.Equal(Orientation.Up, spiderHistory[5].Orientation);

        // Verify intermediate positions
        Assert.Equal(2, spiderHistory[1].X);
        Assert.Equal(5, spiderHistory[1].Y);
        Assert.Equal(Orientation.Up, spiderHistory[1].Orientation);

        Assert.Equal(2, spiderHistory[2].X);
        Assert.Equal(5, spiderHistory[2].Y);
        Assert.Equal(Orientation.Left, spiderHistory[2].Orientation);

        Assert.Equal(1, spiderHistory[3].X);
        Assert.Equal(5, spiderHistory[3].Y);
        Assert.Equal(Orientation.Left, spiderHistory[3].Orientation);

        Assert.Equal(0, spiderHistory[4].X);
        Assert.Equal(5, spiderHistory[4].Y);
        Assert.Equal(Orientation.Left, spiderHistory[4].Orientation);
    }
}
