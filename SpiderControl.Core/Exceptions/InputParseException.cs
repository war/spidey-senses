using FluentValidation.Results;

namespace SpiderControl.Core.Exceptions;

public class InputParseException : Exception
{
    public InputParseException(string message) : base(message) {}

    public InputParseException(string message, Exception innerException)
        : base(message, innerException) {}
}
