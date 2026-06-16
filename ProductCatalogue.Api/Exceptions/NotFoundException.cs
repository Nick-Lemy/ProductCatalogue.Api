namespace ProductCatalogue.Api.Exceptions;

public class NotFoundException(string message) : Exception(message)
{
}