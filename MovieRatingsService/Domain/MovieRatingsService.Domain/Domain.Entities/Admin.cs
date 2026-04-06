using Domain.ValueObject;
using Domain.ValueObject.Exceptions;

namespace MovieRatingsService.Domain.Domain.Entities
{
    // Администратор системы. Управляет фильмами и модерирует контент.
    public class Admin
    {
        public Guid Id { get; private set; }
        public Username Username { get; private set; }
        public PasswordHash PasswordHash { get; private set; }
        public Permissions Permissions { get; private set; }

        public Admin(Username username, PasswordHash passwordHash, Permissions permissions)
        {
            Id = Guid.NewGuid();
            Username = username ?? throw new DomainArgumentNullException(nameof(username));
            PasswordHash = passwordHash ?? throw new DomainArgumentNullException(nameof(passwordHash));
            Permissions = permissions ?? throw new DomainArgumentNullException(nameof(permissions));
        }

        public override string ToString() => $"{Username.Value} (Админ)";

        // Навигационные коллекции
        public virtual ICollection<Movie> CreatedMovies { get; private set; } = new List<Movie>();
        public virtual ICollection<Rating> DeletedRatings { get; private set; } = new List<Rating>();

        public void UpdateUsername(Username newUsername) => Username = newUsername;
        public void UpdatePasswordHash(PasswordHash newHash) => PasswordHash = newHash;
        public void UpdatePermissions(Permissions newPermissions) => Permissions = newPermissions;

        public bool HasPermission(string permission)
        {
            return Permissions.Value.Split(',').Contains(permission);
        }

        // Создать новый фильм.
        public Movie CreateMovie(Title title, string? description = null, int? releaseYear = null, string? genre = null, string? posterUrl = null)
        {
            var movie = new Movie(title, this, description, releaseYear, genre, posterUrl);
            CreatedMovies.Add(movie);
            return movie;
        }

        // Обновить информацию о фильме.
        public void UpdateMovie(Movie movie, Title newTitle, string? newDescription,
                                int? newReleaseYear, string? newGenre, string? newPosterUrl)
        {
            if (movie == null) throw new DomainArgumentNullException(nameof(movie));
            if (movie.CreatedByAdmin != this)
                throw new UnauthorizedOperationException("Только создавший администратор может редактировать фильм.");
            movie.UpdateInfo(newTitle, newDescription, newReleaseYear, newGenre, newPosterUrl);
        }

        // Удалить фильм.
        public void DeleteMovie(Movie movie)
        {
            if (movie == null) throw new DomainArgumentNullException(nameof(movie));
            if (movie.CreatedByAdmin != this)
                throw new UnauthorizedOperationException("Только создавший администратор может удалить фильм.");
            movie.MarkAsDeleted();
        }

        // Скрыть рецензию.
        public void HideReview(Review review)
        {
            if (review == null) throw new DomainArgumentNullException(nameof(review));
            review.Hide(this);
        }
        // Восстановить рецензию.
        public void UnhideReview(Review review)
        {
            if (review == null) throw new DomainArgumentNullException(nameof(review));
            review.Unhide();
        }

        // Удалить оценку.
        public void DeleteRating(Rating rating)
        {
            if (rating == null) throw new DomainArgumentNullException(nameof(rating));
            rating.Delete(this);
            DeletedRatings.Add(rating);
        }
    }
}