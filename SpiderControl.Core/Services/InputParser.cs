﻿using SpiderControl.Core.Factories;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Core.Services;

public class InputParser : IInputParser
{
    private readonly ICommandFactory _commandFactory;

    public InputParser()
    {
        _commandFactory = new CommandFactory();
    }

    public IEnumerable<ICommand> ParseCommands(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentException("Commands cannot be empty");
        }

        return input.Select(x => _commandFactory.CreateCommand(x));
    }

    public SpiderModel ParseSpiderPosition(string input)
    {
        return new SpiderModel(1, 1, Enums.Orientation.Up);
    }

    public WallModel ParseWallDimensions(string input)
    {
        var args = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (args.Length != 2)
        {
            throw new ArgumentException("Invalid wall dimension. Expected: 'width' 'height'");
        }

        int width, height;
        if (!int.TryParse(args[0], out width))
        {
            throw new ArgumentException("Invalid wall width. Expected: 'width height'");
        } 
        else if (!int.TryParse(args[1], out height))
        {
            throw new ArgumentException("Invalid wall height. Expected: 'width height'");
        }

        if (width < 0)
        {
            throw new ArgumentException("Invalid wall width. Wall dimensions should be above 0");
        }
        else if (height < 0)
        {
            throw new ArgumentException("Invalid wall height. Wall dimensions should be above 0");
        }

        return new WallModel(width, height);
    }
}
