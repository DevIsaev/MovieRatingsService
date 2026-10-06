namespace Domain.ValueObject.Exceptions
{
    // ValueObject null или пустое
    public class ValueNullException : DomainArgumentNullException
    {
        public ValueNullException(string paramName)
            : base(paramName, $"Значение '{paramName}' не может быть null или состоять только из пробелов.") { }
    }
}
