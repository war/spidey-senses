using FluentValidation;
using SpiderControl.Core.Models;
using System.Runtime.CompilerServices;

namespace SpiderControl.Core.Validators;

public class CommandValidator : AbstractValidator<char>
{
    // TODO move this to config
    private static readonly HashSet<char> ValidCommands = new(new[] { 'F', 'R', 'L' });

    public CommandValidator()
    {
        RuleFor(x => x)
            .Must(IsValidCommand)
            .WithMessage($"Command must be one of: { string.Join(", ", ValidCommands) }");
    }

    private bool IsValidCommand(char command) => ValidCommands.Contains(char.ToUpper(command));
}
