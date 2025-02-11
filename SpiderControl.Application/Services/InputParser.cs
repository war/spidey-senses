using SpiderControl.Application.Interfaces;
using SpiderControl.Core.Enums;
using SpiderControl.Core.Exceptions;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Application.Services;

public class InputParser : IInputParser
{
    private readonly ICommandFactory _commandFactory;
    private readonly IValidatorService _validatorService;

    public InputParser(ICommandFactory commandFactory, IValidatorService validatorService)
    {
        _commandFactory = commandFactory ?? throw new ArgumentNullException();
        _validatorService = validatorService ?? throw new ArgumentNullException();
    }

    public IEnumerable<ICommand> ParseCommands(string input)
    {
        try
        { 
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new InputParseException("Command input cannot be empty");
            }

            var validationResult = _validatorService.ValidateCommands(input);
            if (!validationResult.IsValid)
            {
                throw new ModelValidationException(validationResult);
            }

            var commands = input.Select(x => _commandFactory.CreateCommand(x));

            return commands;
        }
        catch (Exception ex)
        {
            throw new InputParseException($"Failed to parse commands: {ex.Message}", ex);
        }
    }

    public SpiderModel ParseSpiderPosition(string input)
    {
        try
        {
            var args = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (args.Length != 3 ||
                !int.TryParse(args[0], out int x) ||
                !int.TryParse(args[1], out int y) ||
                !Enum.TryParse<Orientation>(args[2], true, out var orientation) || (int)orientation > 3)
            {
                throw new InputParseException("Invalid spider position format. Expected: 'x y orientation' where x and y are integers and orientation is Up/Down/Left/Right");
            }

            var spider = new SpiderModel(x, y, orientation);
            var validationResult = _validatorService.ValidateSpider(spider);

            if (!validationResult.IsValid)
            {
                throw new ModelValidationException(validationResult);
            }

            return new SpiderModel(x, y, orientation);
        }
        catch (Exception ex) when (ex is not InputParseException and not ModelValidationException)
        {
            throw new InputParseException($"Failed to parse spider position: {ex.Message}");
        }
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
