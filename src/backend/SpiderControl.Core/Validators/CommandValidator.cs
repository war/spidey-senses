using FluentValidation;
using Microsoft.Extensions.Options;
using SpiderControl.Core.Configuration;

namespace SpiderControl.Core.Validators;

public class CommandValidator : AbstractValidator<char>
{
    private readonly HashSet<char> _validCommands;

    public CommandValidator(IOptions<SpiderControlConfig> config)
    {
        _validCommands = new HashSet<char>(config.Value.ValidCommands);

        RuleFor(x => x)
            .Must(IsValidCommand)
            .WithMessage($"Command must be one of: { string.Join(", ", _validCommands) }");
    }

    private bool IsValidCommand(char command) => _validCommands.Contains(char.ToUpper(command));
}
