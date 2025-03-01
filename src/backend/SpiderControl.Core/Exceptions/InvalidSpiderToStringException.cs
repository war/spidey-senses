namespace SpiderControl.Core.Exceptions;

public class InvalidSpiderToStringException : Exception
{
    public InvalidSpiderToStringException(string message) : base(message) { }

    public InvalidSpiderToStringException(string message, Exception innerException)
        : base(message, innerException) { }
}
