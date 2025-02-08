using SpiderControl.Core.Models;

namespace SpiderControl.Core.Tests;

public class WallModelTests
{
    [Fact]
    public void Position_AfterInitialisation_ReturnsStartingDimensions()
    {
        // Arrange
        const int width = 15;
        const int height = 16;

        // Act
        var wall = new WallModel(width, height);

        // Assert
        Assert.Equal(width, wall.Width);
        Assert.Equal(height, wall.Height);
    }
}
