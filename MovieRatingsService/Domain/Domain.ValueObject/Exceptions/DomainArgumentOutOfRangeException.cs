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

    // Строка превышает максимальную длину
    public class ValueTooLongException : DomainArgumentOutOfRangeException
    {
        public ValueTooLongException(string paramName, int actualLength, int maxLength)
            : base(paramName, actualLength,
                $"Длина '{paramName}' ({actualLength}) превышает максимально допустимую ({maxLength}).")
        { }
    }

    // Некорректная оценка
    public class InvalidScoreException : DomainArgumentOutOfRangeException
    {
        public InvalidScoreException(int score)
            : base(nameof(score), score,
                $"Оценка должна быть от 1 до 10, но получено {score}.")
        { }
    }
}
