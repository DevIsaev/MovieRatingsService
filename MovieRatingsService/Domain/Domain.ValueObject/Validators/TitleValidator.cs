using Domain.ValueObject.Base;
using Domain.ValueObject.Exceptions;

namespace Domain.ValueObject.Validators
{
    // Валидатор названия фильма: не null, не пустой, длина не больше 200
    public class TitleValidator : IValidator<string>
    {
        private const int MaxLength = 200;
        private const int MinLength = 1;

        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValueNullException(nameof(Title));
            if (value.Length < MinLength)
                throw new ValueTooShortException(nameof(Title), value.Length, MinLength);
            if (value.Length > MaxLength)
                throw new ValueTooLongException(nameof(Title), value.Length, MaxLength);
        }
    }
}
