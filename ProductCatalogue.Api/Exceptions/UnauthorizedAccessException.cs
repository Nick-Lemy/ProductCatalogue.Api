namespace ProductCatalogue.Api.Exceptions;

public class UnauthorizedAccessException(string message) : Exception(message)
{
}