using SpiderControl.Core.Enums;
using SpiderControl.Core.Models;

namespace SpiderControl.Core.Tests.Models;

public class SpiderTests
{
    [Fact]
    public void Position_AfterInitialisation_ReturnsStartingPosition()
    {
        // Arrange
        const int x = 2;
        const int y = 4;

        // Act
        var spider = new Spider(x, y, Orientation.Up);

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
        var spider = new Spider(x, y, orientation);

        // Assert
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
        var spider = new Spider(0, 0, initial);

        // Act
        spider.RotateLeft();

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
        var spider = new Spider(0, 0, initial);

        // Act
        spider.RotateRight();

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
        var spider = new Spider(0, 0, orientation);

        // Act
        spider.MoveForward();

        // Assert
        Assert.Equal(expectedX, spider.X);
        Assert.Equal(expectedY, spider.Y);
        Assert.Equal(orientation, spider.Orientation);
    }
}
