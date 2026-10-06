namespace Domain.ValueObject.Exceptions
{
    // Числовое значение больше максимума
    public class ValueTooBigException : DomainArgumentOutOfRangeException
    {
        public ValueTooBigException(string paramName, int actualValue, int maxValue)
            : base(paramName, actualValue,
                $"Значение '{paramName}' ({actualValue}) больше максимально допустимого ({maxValue}).")
        { }
    }
}
