using Domain.ValueObject.Base;
using Domain.ValueObject.Exceptions;

namespace Domain.ValueObject.Validators
{
    // Валидатор оценки: от 1 до 10
    public class ScoreValidator : IValidator<int>
    {
        private const int Min = 1;
        private const int Max = 10;

        public void Validate(int value)
        {
            if (value < Min)
                throw new SmallScoreException(value);
            if (value > Max)
                throw new BigScoreException(value);
        }
    }
}
