namespace Domain.ValueObject.Exceptions
{
    // Базовое исключение для всех ошибок
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
        public DomainException(string message, Exception innerException) : base(message, innerException) { }
    }
}
