using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject
{
    // Название фильма. Не может быть пустым и не длиннее 200 символов
    public class Title(string value) : ValueObject<string>(new TitleValidator(), value);
}
