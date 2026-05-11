namespace Domain.ValueObject.Exceptions
{
    // Строка превышает максимальную длину
    public class ValueTooLongException : DomainArgumentOutOfRangeException
    {
        public ValueTooLongException(string paramName, int actualLength, int maxLength)
            : base(paramName, actualLength,
                $"Длина '{paramName}' ({actualLength}) превышает максимально допустимую ({maxLength}).")
        { }
    }
}
