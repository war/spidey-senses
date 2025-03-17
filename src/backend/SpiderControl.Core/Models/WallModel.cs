namespace SpiderControl.Core.Models;
 
public class WallModel
{
    public int Width { get; set; }
    public int Height { get; set; }

    public WallModel(int width, int height)
    {
        Width = width;
        Height = height;
    }
}
