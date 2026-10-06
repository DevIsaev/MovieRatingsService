using Domain.ValueObject.Base;
using Domain.ValueObject.Exceptions;

namespace Domain.ValueObject.Validators
{
    // Валидатор Url ссылки на постер
    public class PosterUrlValidator : IValidator<string>
    {
        public const int MaxLength = 500;

        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValueNullException(nameof(PosterUrl));
            if (value.Length > MaxLength)
                throw new ValueTooLongException(nameof(PosterUrl), value.Length, MaxLength);
            if (!Uri.TryCreate(value, UriKind.Absolute, out _))
                throw new InvalidUrlException(nameof(PosterUrl), value);
        }
    }
}
