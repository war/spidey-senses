using SpiderControl.Core.Models;

namespace SpiderControl.Core.Tests;

public class SpiderTests
{
    [Fact]
    public void Position_AfterInitialisation_ReturnsStartingPosition()
    {
        // Arrange
        const int x = 2;
        const int y = 4;

        // Act
        var spider = new SpiderModel(x, y);

        // Assert
        Assert.Equal(2, spider.X);
        Assert.Equal(4, spider.Y);
    }

    [Fact]
    public void Orientation_AfterInitialisation_ReturnsStartingOrientation()
    {
        // Arrange
        const int x = 2;
        const int y = 4;
        const Orientation orientation = Orientation.Right;

        // Act
        var spider = new SpiderModel(x, y);

        // Assert
        Assert.Equal(Orientation.Right, spider.Orientation);
    }


}
