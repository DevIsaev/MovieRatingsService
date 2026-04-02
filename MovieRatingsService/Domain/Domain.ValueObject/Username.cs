using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject
{
    // Имя пользователя. Не может быть пустым и не длиннее 50 символов.
    public class Username : ValueObject<string>
    {
        private static readonly IValidator<string> _defaultValidator = new UsernameValidator();

        public Username(string value) : this(_defaultValidator, value) { }
        public Username(IValidator<string> validator, string value) : base(validator, value) { }
    }
}
