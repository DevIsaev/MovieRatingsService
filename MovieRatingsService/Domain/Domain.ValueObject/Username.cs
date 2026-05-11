using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject
{
    // Имя пользователя. Не может быть пустым и не длиннее 50 символов.
    public class Username(string value) : ValueObject<string>(new UsernameValidator(), value);
}
