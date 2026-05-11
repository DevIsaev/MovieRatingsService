namespace Domain.ValueObject.Exceptions
{
    // Числовое значение меньше минимума
    public class ValueTooSmallException : DomainArgumentOutOfRangeException
    {
        public ValueTooSmallException(string paramName, int actualValue, int minValue)
            : base(paramName, actualValue,
                $"Значение '{paramName}' ({actualValue}) меньше минимально допустимого ({minValue}).")
        { }
    }
}
