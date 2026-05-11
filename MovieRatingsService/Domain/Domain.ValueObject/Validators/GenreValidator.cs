using Domain.ValueObject.Base;
using Domain.ValueObject.Exceptions;

namespace Domain.ValueObject.Validators
{
    // Валидатор жанра
    public class GenreValidator : IValidator<string>
    {
        private const int MaxLength = 100;
        private const int MinLength = 1;

        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValueNullException(nameof(Genre));
            if (value.Length < MinLength)
                throw new ValueTooShortException(nameof(Genre), value.Length, MinLength);
            if (value.Length > MaxLength)
                throw new ValueTooLongException(nameof(Genre), value.Length, MaxLength);
        }
    }
}
