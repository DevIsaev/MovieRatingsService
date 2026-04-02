using Domain.ValueObject.Base;
using Domain.ValueObject.Exceptions;

namespace Domain.ValueObject.Validators
{
    // Валидатор имени пользователя: не null, не пустой, длина не больше 50 символов
    public class UsernameValidator : IValidator<string>
    {
        public static int MaxLength => 50;

        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValueNullException(nameof(value));
            if (value.Length > MaxLength)
                throw new ValueTooLongException(nameof(value), value.Length, MaxLength);
        }
    }
}
