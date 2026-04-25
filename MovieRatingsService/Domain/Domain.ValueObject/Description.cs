using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject
{
    // Описание фильма, не может быть длиннее 1000 символов
    public class Description(string? value) : ValueObject<string?>(new DescriptionValidator(), value);
}
