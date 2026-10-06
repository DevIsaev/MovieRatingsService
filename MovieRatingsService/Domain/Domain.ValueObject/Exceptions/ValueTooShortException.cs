namespace Domain.ValueObject.Exceptions
{
    // Строка имеет меньшую длину
    public class ValueTooShortException : DomainArgumentOutOfRangeException
    {
        public ValueTooShortException(string paramName, int actualLength, int minLength)
            : base(paramName, actualLength,
                $"Длина '{paramName}' ({actualLength}) слишком малая ({minLength}).")
        { }
    }
}
