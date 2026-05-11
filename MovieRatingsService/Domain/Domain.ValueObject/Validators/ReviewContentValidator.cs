using Domain.ValueObject.Base;
using Domain.ValueObject.Exceptions;

namespace Domain.ValueObject.Validators
{
    // Валидатор текста рецензии: не null и не больше 2000 символов 
    public class ReviewContentValidator : IValidator<string>
    {
        private const int MaxLength = 2000;
        private const int MinLength = 1;

        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValueNullException(nameof(ReviewContent));
            if (value.Length < MinLength)
                throw new ValueTooShortException(nameof(ReviewContent), value.Length, MinLength);
            if (value.Length > MaxLength)
                throw new ValueTooLongException(nameof(ReviewContent), value.Length, MaxLength);
        }
    }
}
