using SpiderControl.Core.Commands;
using SpiderControl.Core.Enums;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;
using SpiderControl.Core.Services;

namespace SpiderControl.Core.Tests.Services;

public class SpiderServiceTests
{
    private readonly ISpiderService _spiderService;

    public SpiderServiceTests()
    {
        _spiderService = new SpiderService();
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

    [Theory]
    [InlineData(Orientation.Up, Orientation.Left)]
    [InlineData(Orientation.Left, Orientation.Down)]
    [InlineData(Orientation.Down, Orientation.Right)]
    [InlineData(Orientation.Right, Orientation.Up)]
    public void RotateLeft_RotatesCorrectly(Orientation initial, Orientation expected)
    {
        // Arrange
        var spider = new SpiderModel(0, 0, initial);

        // Act
        _spiderService.RotateLeft(spider);

        // Assert
        Assert.Equal(expected, spider.Orientation);
    }

    [Theory]
    [InlineData(Orientation.Up, Orientation.Right)]
    [InlineData(Orientation.Right, Orientation.Down)]
    [InlineData(Orientation.Down, Orientation.Left)]
    [InlineData(Orientation.Left, Orientation.Up)]
    public void RotateRight_RotatesCorrectly(Orientation initial, Orientation expected)
    {
        // Arrange
        var spider = new SpiderModel(0, 0, initial);

        // Act
        _spiderService.RotateRight(spider);

        // Assert
        Assert.Equal(expected, spider.Orientation);
    }

    [Theory]
    [InlineData(Orientation.Up, 0, 1)]
    [InlineData(Orientation.Right, 1, 0)]
    [InlineData(Orientation.Down, 0, -1)]
    [InlineData(Orientation.Left, -1, 0)]
    public void MoveForward_MovesInCorrectDirection(Orientation orientation, int expectedX, int expectedY)
    {
        // Arrange
        var spider = new SpiderModel(0, 0, orientation);

        // Act
        _spiderService.MoveForward(spider);

        // Assert
        Assert.Equal(expectedX, spider.X);
        Assert.Equal(expectedY, spider.Y);
        Assert.Equal(orientation, spider.Orientation);
    }

    [Fact]
    public void ProcessCommands_ExecutesAllCommands()
    {
        // Arrange
        var spider = new SpiderModel(2, 4, Orientation.Up);
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
