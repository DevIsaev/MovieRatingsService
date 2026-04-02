using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject
{
    // Оценка фильма. Допустимые значения: от 1 до 10.
    public class Score:ValueObject<int>
    {
        private static readonly IValidator<int> _validator = new ScoreValidator();

        // Создаёт оценку со значением
        public Score(int value) : this(_validator, value) { }

        public Score(IValidator<int> validator, int value) : base(validator, value) { }
    }
}
