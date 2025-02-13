using SpiderControl.Application.Interfaces;
using SpiderControl.Core.Enums;
using SpiderControl.Core.Exceptions;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Application.Services;

public class CommandInputParser : ICommandInputParser
{
    private readonly ICommandFactory _commandFactory;
    private readonly IValidatorService _validatorService;

    public CommandInputParser(ICommandFactory commandFactory, IValidatorService validatorService)
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
}
