using SpiderControl.Core.Models;

namespace SpiderControl.Core.Tests;

public class SpiderTests
{
    [Fact]
    public void Position_AfterInitialization_ReturnsStartingPosition()
    {
        var spider = new SpiderModel();

        Assert.Equal(2, spider.X);
        Assert.Equal(4, spider.Y);
    }
}
