using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject
{
    // Название фильма. Не может быть пустым и не длиннее 200 символов.
    public class Title : ValueObject<string>
    {
        private static readonly IValidator<string> _defaultValidator = new TitleValidator();

        public Title(string value) : this(_defaultValidator, value) { }
        public Title(IValidator<string> validator, string value) : base(validator, value) { }
    }
}
