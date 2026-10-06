namespace Domain.ValueObject.Exceptions
{
    // Некорректные данные фильма
    public class InvalidMovieDataException(string details) : DomainException(details);
}
