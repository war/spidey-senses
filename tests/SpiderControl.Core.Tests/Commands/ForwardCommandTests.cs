using Microsoft.Extensions.Logging;
using Moq;
using SpiderControl.Core.Commands;
using SpiderControl.Core.Enums;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;
using SpiderControl.Core.Services;

namespace SpiderControl.Core.Tests.Commands;

public class ForwardCommandTests
{
    private readonly ICommand _command;
    private readonly WallModel _wall;

    public ForwardCommandTests()
    {
        _command = new ForwardCommand();
        _wall = new WallModel(3, 15);
    }

    [Fact]
    public void Execute_ValidMove_MovesForward()
    {
        // Arrange
        var spider = new Spider(3, 4, Orientation.Up);

        // Act
        var result = _command.Execute(spider, _wall);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(3, spider.X);
        Assert.Equal(5, spider.Y);
        Assert.Equal(Orientation.Up, spider.Orientation);
    }

    [Fact]
    public void Execute_InvalidMove_ReturnsFailure()
    {
        // Arrange
        var spider = new Spider(3, 15, Orientation.Up);

        // Act
        var result = _command.Execute(spider, _wall);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("fall off", result.Error);
    }
}
