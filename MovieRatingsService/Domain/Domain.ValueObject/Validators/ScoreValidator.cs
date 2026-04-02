using Domain.ValueObject.Base;
using Domain.ValueObject.Exceptions;

namespace Domain.ValueObject.Validators
{
    // Валидатор оценки: от 1 до 10
    public class ScoreValidator : IValidator<int>
    {
        public void Validate(int value)
        {
            if (value < 1 || value > 10)
                throw new InvalidScoreException(value);
        }
    }
}
