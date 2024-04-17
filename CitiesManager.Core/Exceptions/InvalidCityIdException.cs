namespace CitiesManager.Core.Exceptions;

public class InvalidCityIdException : ArgumentNullException
{
    public InvalidCityIdException()
    {
    }

    public InvalidCityIdException(string? message) : base(message)
    {
    }

    public InvalidCityIdException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}