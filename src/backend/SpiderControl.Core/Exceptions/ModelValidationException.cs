using FluentValidation.Results;

namespace SpiderControl.Core.Exceptions;

public class ModelValidationException : Exception
{
    public ValidationResult ValidationResult { get; }

    public ModelValidationException(ValidationResult validationResult) : base(FormatMessage(validationResult))
    {
        ValidationResult = validationResult;
    }

    public static string FormatMessage(ValidationResult validationResult)
    {
        var errors = validationResult.Errors
            .Select(e => $"- {e.PropertyName}: {e.ErrorMessage}")
            .ToList();

        return $"Validation failed:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}";
    }
}
