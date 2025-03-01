using SpiderControl.Core.Models;

namespace SpiderControl.Core.Tests.Models;

public class WallModelTests
{
    [Fact]
    public void Dimensions_AfterInitialisation_ReturnsStartingDimensions()
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
