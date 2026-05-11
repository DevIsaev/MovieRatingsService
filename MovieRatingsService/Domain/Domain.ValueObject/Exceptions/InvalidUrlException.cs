namespace Domain.ValueObject.Exceptions
{
    public class InvalidUrlException : DomainArgumentOutOfRangeException
    {
        public InvalidUrlException(string paramName, string actualValue)
            : base(paramName, actualValue,
                $"Значение '{paramName}' не является валидным абсолютным URL: '{actualValue}'.")
        { }
    }
}
