using SpiderControl.Application.Interfaces;
using SpiderControl.Core.Enums;
using SpiderControl.Core.Exceptions;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Application.Services;

public class WallInputParser : IWallInputParser
{
    private readonly IValidatorService _validatorService;

    public WallInputParser(IValidatorService validatorService)
    {
        _validatorService = validatorService ?? throw new ArgumentNullException();
    }

    public WallModel ParseWallDimensions(string input)
    {
        var args = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (args.Length != 2)
        {
            throw new InputParseException("Invalid wall dimensions format. Expected: 'width height' where width and height are integers");
        }

        int width, height;
        if (!int.TryParse(args[0], out width))
        {
            throw new InputParseException("Invalid wall width. Expected: 'width height'");
        } 
        else if (!int.TryParse(args[1], out height))
        {
            throw new InputParseException("Invalid wall height. Expected: 'width height'");
        }

        var wall = new WallModel(width, height);
        var validationResult = _validatorService.ValidateWall(wall);

        return wall;
    }
}
