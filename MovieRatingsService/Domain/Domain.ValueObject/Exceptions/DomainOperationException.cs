namespace Domain.ValueObject.Exceptions
{
    public class DomainOperationException : InvalidOperationException
    {
        public DomainOperationException(string message) : base(message) { }
        public DomainOperationException(string message, Exception innerException) : base(message, innerException) { }
    }
}