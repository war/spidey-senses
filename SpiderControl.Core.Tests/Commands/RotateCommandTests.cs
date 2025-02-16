using Microsoft.Extensions.Logging;
using Moq;
using SpiderControl.Core.Commands;
using SpiderControl.Core.Enums;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;
using SpiderControl.Core.Services;
using System;
namespace SpiderControl.Core.Tests.Commands;

public class RotateCommandTests
{
    private readonly ISpiderService _spiderService;
    private readonly Mock<ILogger<SpiderService>> _loggerMock;

    private readonly ICommand _rotateRightCommand;
    private readonly ICommand _rotateLeftCommand;

    private readonly WallModel _wall;

    public RotateCommandTests()
    {
        _loggerMock = new Mock<ILogger<SpiderService>>();
        _spiderService = new SpiderService(_loggerMock.Object);

        _rotateRightCommand = new RotateRightCommand();
        _rotateLeftCommand = new RotateLeftCommand();

        _wall = new WallModel(3, 15);
    }

    [Fact]
    public void Execute_RotateLeft_RotatesCorrectly()
    {
        // Arrange
        var spider = new Spider(2, 2, Orientation.Up);

        // Act
        _rotateLeftCommand.Execute(spider, _wall, _spiderService);

        // Assert
        Assert.Equal(2, spider.X);
        Assert.Equal(2, spider.Y);
        Assert.Equal(Orientation.Left, spider.Orientation);
    }

    [Fact]
    public void Execute_RotateRight_RotatesCorrectly()
    {
        // Arrange
        var spider = new Spider(2, 2, Orientation.Up);

        // Act
        _rotateRightCommand.Execute(spider, _wall, _spiderService);

        // Assert
        Assert.Equal(2, spider.X);
        Assert.Equal(2, spider.Y);
        Assert.Equal(Orientation.Right, spider.Orientation);
    }
}
