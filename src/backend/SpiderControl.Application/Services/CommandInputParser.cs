using SpiderControl.Application.Interfaces;
using SpiderControl.Core.Common;
using SpiderControl.Core.Interfaces;

namespace SpiderControl.Application.Services;

public class CommandInputParser : ICommandInputParser
{
    private readonly ICommandFactory _commandFactory;
    private readonly IValidatorService _validatorService;

    public CommandInputParser(ICommandFactory commandFactory, IValidatorService validatorService)
    {
        _commandFactory = commandFactory;
        _validatorService = validatorService;
    }

    public Result<IEnumerable<ICommand>> ParseCommands(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return Result<IEnumerable<ICommand>>.Failure("Command input cannot be empty");

        var validationResult = _validatorService.ValidateCommands(input);

        if (!validationResult.IsSuccess)
            return Result<IEnumerable<ICommand>>.Failure(validationResult.Error);

        var commands = new List<ICommand>();

        foreach (var c in input)
        {
            var commandResult = _commandFactory.CreateCommand(c);
            if (!commandResult.IsSuccess)
                return Result<IEnumerable<ICommand>>.Failure(commandResult.Error);

            commands.Add(commandResult.Value);
        }

        return Result<IEnumerable<ICommand>>.Success(commands);
    }
}
