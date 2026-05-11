using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject
{
    // Текст рецензии. Не может быть пустым и не длиннее 2000 символов.
    public class ReviewContent(string value) : ValueObject<string>(new ReviewContentValidator(), value);
}
