using Domain.ValueObject.Base;
using Domain.ValueObject.Exceptions;

namespace Domain.ValueObject.Validators
{
    // Валидатор текста рецензии: не null и не больше 2000 символов 
    public class ReviewContentValidator : IValidator<string>
    {
        public static int MaxLength => 2000;

        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValueNullException(nameof(value));
            if (value.Length > MaxLength)
                throw new ValueTooLongException(nameof(value), value.Length, MaxLength);
        }
    }
}
