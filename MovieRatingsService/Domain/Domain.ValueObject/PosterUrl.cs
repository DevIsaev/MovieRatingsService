using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject
{
    // URL постера фильма, не длиннее 500 символов
    public class PosterUrl(string value) : ValueObject<string>(new PosterUrlValidator(), value);
}
