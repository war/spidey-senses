using SpiderControl.Core.Commands;
using SpiderControl.Core.Enums;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;
using SpiderControl.Core.Services;
using System;
namespace SpiderControl.Core.Tests.Commands;

public class ForwardCommandTests
{
    private readonly ISpiderService _spiderService;
    private readonly ICommand _command;
    private readonly WallModel _wall;

    public ForwardCommandTests()
    {
        _spiderService = new SpiderService();
        _command = new ForwardCommand();
        _wall= new WallModel(3, 15);
    }

    [Fact]
    public void Execute_ValidMove_MovesForward()
    {
        // Arrange
        var spider = new SpiderModel(3, 4, Orientation.Up);

        // Act
        _command.Execute(spider, _wall, _spiderService);

        // Assert
        Assert.Equal(3, spider.X);
        Assert.Equal(5, spider.Y);
        Assert.Equal(Orientation.Up, spider.Orientation);
    }

    [Fact]
    public void Execute_InvalidMove_ThrowsException()
    {
        var spider = new SpiderModel(3, 4, Orientation.Up);

        Assert.Throws<InvalidOperationException>(() => _command.Execute(spider, _wall, _spiderService));
    }
}
