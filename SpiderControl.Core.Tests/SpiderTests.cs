using SpiderControl.Core.Models;

namespace SpiderControl.Core.Tests;

public class SpiderTests
{
    [Fact]
    public void Position_AfterInitialization_ReturnsStartingPosition()
    {
        // Arrange & Act
        var spider = new SpiderModel(x: 2, y: 4);

        // Assert
        Assert.Equal(2, spider.X);
        Assert.Equal(4, spider.Y);
    }
}
