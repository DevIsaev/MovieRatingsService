using Domain.ValueObject.Base;
using Domain.ValueObject.Validators;

namespace Domain.ValueObject
{
    // Год выпуска фильма, (1888 - текущий год)
    public class ReleaseYear(int value) : ValueObject<int>(new ReleaseYearValidator(), value);
}
