using Domain.ValueObject.Base;
using Domain.ValueObject.Exceptions;

namespace Domain.ValueObject.Validators
{
    // Валидатор имени пользователя: не null, не пустой, длина не больше 50 символов
    public class UsernameValidator : IValidator<string>
    {
        public static int MaxLength => 50;
        public static int MinLength => 2;

        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValueNullException(nameof(Username));
            if (value.Length < MinLength)
                throw new ValueTooShortException(nameof(Username), value.Length, MinLength);
            if (value.Length > MaxLength)
                throw new ValueTooLongException(nameof(Username), value.Length, MaxLength);
        }
    }
}
