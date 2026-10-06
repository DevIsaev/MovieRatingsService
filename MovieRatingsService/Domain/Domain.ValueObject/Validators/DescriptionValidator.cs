using Domain.ValueObject.Base;
using Domain.ValueObject.Exceptions;

namespace Domain.ValueObject.Validators
{
    // Валидатор описания фильма
    public class DescriptionValidator : IValidator<string?>
    {
        public const int MaxLength = 1000;

        public void Validate(string? value)
        {
            if (value is null) return;
            if (value.Length > MaxLength)
                throw new ValueTooLongException(nameof(Description), value.Length, MaxLength);
        }
    }
}
