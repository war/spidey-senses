namespace SpiderControl.Console.Models;

public class InputModel
{
    public string WallDimensions { get; set; }
    public string SpiderPosition { get; set; }
    public string Commands { get; set; }

    public InputModel(string wallDimensions, string spiderPosition, string commands)
    {
        WallDimensions = wallDimensions;
        SpiderPosition = spiderPosition;
        Commands = commands;
    }
}
