using MovieRatingsService.Domain.Domain.Entities;

namespace Domain.ValueObject.Exceptions
{
    // Фильм с таким названием уже существует у этого администратора
    public class DuplicateMovieTitleException(Admin admin, Title title)
        : DomainOperationException(
            $"Администратор {admin.Username.Value} уже создал фильм с названием \"{title.Value}\".")
    {
        public Admin Admin => admin;
        public Title Title => title;
    }
}
