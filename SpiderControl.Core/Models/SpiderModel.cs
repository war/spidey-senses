namespace SpiderControl.Core.Models;

public class SpiderModel
{
    public int X { get; set; }
    public int Y { get; set; }
    public Orientation Orientation { get; set; }

    public SpiderModel(int x, int y, Orientation orientation)
    {
        X = x; 
        Y = y;
        Orientation = orientation;
    }
}
