using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject
{
    // Оценка фильма. Допустимые значения: от 1 до 10
    public class Score(int value) : ValueObject<int>(new ScoreValidator(), value);
}
