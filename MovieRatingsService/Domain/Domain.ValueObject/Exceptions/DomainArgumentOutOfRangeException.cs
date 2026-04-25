namespace Domain.ValueObject.Exceptions
{
    // Исключения для аргумента вне допустимого диапазона
    public class DomainArgumentOutOfRangeException : ArgumentOutOfRangeException
    {
        public DomainArgumentOutOfRangeException(string paramName, object? actualValue, string message)
            : base(paramName, actualValue, message) { }

        public DomainArgumentOutOfRangeException(string paramName, string message)
            : base(paramName, message) { }
    }
}
