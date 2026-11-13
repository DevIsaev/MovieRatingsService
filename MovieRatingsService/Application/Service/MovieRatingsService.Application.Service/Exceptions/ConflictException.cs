
namespace MovieRatingsService.Application.Service.Exceptions
{
    //конфликт на уровне приложения - HTTP 409
    public class ConflictException(string message) : Exception(message);
}
