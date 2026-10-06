namespace Domain.ValueObject.Exceptions
{
    // Исключение для недопустимых операций
    public class DomainOperationException : DomainException
    {
        protected DomainOperationException(string message) : base(message) { }
    }
}