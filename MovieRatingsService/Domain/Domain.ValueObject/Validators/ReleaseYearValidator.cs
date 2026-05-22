using Domain.ValueObject.Base;
using Domain.ValueObject.Exceptions;


namespace Domain.ValueObject.Validators
{
    // Валидатор года выхода фильма
    public class ReleaseYearValidator : IValidator<int>
    {

        public const int MinYear = 1888;

        public void Validate(int value)
        {
            int maxYear = DateTime.UtcNow.Year + 5; // допускаем фильмы в ближайшем будущем
            if (value < MinYear)
                throw new ValueTooSmallException(nameof(ReleaseYear), value, MinYear);
            if (value > maxYear)
                throw new ValueTooBigException(nameof(ReleaseYear), value, maxYear);
        }
    }
}
