namespace Domain.ValueObject.Exceptions
{
    // Некорректная оценка - отрицательная или низкая
    public class SmallScoreException : DomainArgumentOutOfRangeException
    {
        public SmallScoreException(int score)
            : base(nameof(score), score,
                $"Оценка должна быть не меньше 1, но получено {score}.")
        { }
    }
}
