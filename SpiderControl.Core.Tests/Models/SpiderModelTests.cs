using SpiderControl.Core.Enums;
using SpiderControl.Core.Models;

namespace SpiderControl.Core.Tests.Models;

public class SpiderModelTests
{
    [Fact]
    public void Position_AfterInitialisation_ReturnsStartingPosition()
    {
        // Arrange
        const int x = 2;
        const int y = 4;

        // Act
        var spider = new SpiderModel(x, y, Orientation.Up);

        // Assert
        Assert.Equal(x, spider.X);
        Assert.Equal(y, spider.Y);
    }

    [Fact]
    public void Orientation_AfterInitialisation_ReturnsStartingOrientation()
    {
        // Arrange
        const int x = 2;
        const int y = 4;
        const Orientation orientation = Orientation.Right;

        // Act
        var spider = new SpiderModel(x, y, orientation);

        // Assert
        Assert.Equal(Orientation.Right, spider.Orientation);
    }
}
