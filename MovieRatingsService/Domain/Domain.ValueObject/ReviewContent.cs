using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject
{
    // Текст рецензии. Не может быть пустым и не длиннее 2000 символов.
    public class ReviewContent : ValueObject<string>
    {
        private static readonly IValidator<string> _defaultValidator = new ReviewContentValidator();

        //Создаёт рецензию с текстом
        public ReviewContent(string value) : this(_defaultValidator, value) { }

        public ReviewContent(IValidator<string> validator, string value) : base(validator, value) { }
    }
}
