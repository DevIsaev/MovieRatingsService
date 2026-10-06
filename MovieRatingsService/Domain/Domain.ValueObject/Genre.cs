using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject
{
    // Жанр фильма, не может быть пустым и не длиннее 100 символов
    public class Genre(string value) : ValueObject<string>(new GenreValidator(), value);
}
