namespace ProductCatalogue.Api.Exceptions;

public class ValidationException(string message) : Exception(message)
{
}