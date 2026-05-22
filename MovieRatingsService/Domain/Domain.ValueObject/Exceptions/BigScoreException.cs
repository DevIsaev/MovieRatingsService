
namespace Domain.ValueObject.Exceptions
{
    // Некорректная оценка - слишком высокая оценка
    public class BigScoreException : DomainArgumentOutOfRangeException
    {
        public BigScoreException(int score, int max)
            : base(nameof(score), score,
                $"Оценка должна быть не больше {max}, но получено {score}.")
        { }
    }
}
