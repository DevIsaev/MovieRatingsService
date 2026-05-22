namespace Domain.ValueObject.Exceptions
{
    // Некорректная оценка - отрицательная или низкая
    public class SmallScoreException : DomainArgumentOutOfRangeException
    {
        public SmallScoreException(int score,int min)
            : base(nameof(score), score,
                $"Оценка должна быть не меньше {min}, но получено {score}.")
        { }
    }
}
