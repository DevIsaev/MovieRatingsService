namespace Domain.ValueObject.Exceptions
{
    // Исключение для недопустимых операций
    public class DomainOperationException : InvalidOperationException
    {
        public DomainOperationException(string message) : base(message) { }
        public DomainOperationException(string message, Exception innerException) : base(message, innerException) { }
    }
}