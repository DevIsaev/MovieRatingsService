using Domain.ValueObject.Base;
using Domain.ValueObject.Exceptions;

namespace Domain.ValueObject.Validators
{
    // Валидатор хеша пароля: не может быть null или пустым
    public class PasswordHashValidator : IValidator<string>
    {
        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValueNullException(nameof(value));
        }
    }
}
