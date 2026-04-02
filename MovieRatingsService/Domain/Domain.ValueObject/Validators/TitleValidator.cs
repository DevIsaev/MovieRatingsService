using Domain.ValueObject.Base;
using Domain.ValueObject.Exceptions;

namespace Domain.ValueObject.Validators
{
    // Валидатор названия фильма: не null, не пустой, длина не больше 200
    public class TitleValidator : IValidator<string>
    {
        public static int MaxLength => 200;

        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValueNullException(nameof(value));
            if (value.Length > MaxLength)
                throw new ValueTooLongException(nameof(value), value.Length, MaxLength);
        }
    }
}
