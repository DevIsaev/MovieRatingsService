using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject
{
    // Хеш пароля администратора. Не может быть пустым.
    public class PasswordHash : ValueObject<string>
    {
        private static readonly IValidator<string> _defaultValidator = new PasswordHashValidator();

        public PasswordHash(string value) : this(_defaultValidator, value) { }
        public PasswordHash(IValidator<string> validator, string value) : base(validator, value) { }
    }
}
