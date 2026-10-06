namespace Domain.ValueObject.Exceptions
{
    // Базовое исключение для всех ошибок
    public class DomainException : Exception
    {
        protected DomainException(string message) : base(message) { }
    }
}
