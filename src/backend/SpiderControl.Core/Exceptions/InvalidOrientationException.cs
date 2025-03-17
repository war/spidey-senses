namespace SpiderControl.Core.Exceptions;

public class InvalidOrientationException : Exception
{
    public InvalidOrientationException(string message) : base(message) { }

    public InvalidOrientationException(string message, Exception innerException)
        : base(message, innerException) { }
}
