using Domain.ValueObject;
using Domain.ValueObject.Exceptions;
using MovieRatingsService.Domain.Base;

namespace MovieRatingsService.Domain.Domain.Entities
{
    // Администратор системы Управляет фильмами и модерирует контент
    public class Admin : Entity<Guid>
    {
        public Username Username { get; private set; }
        public Permissions Permissions { get; private set; }

        protected Admin() : base() { Username = null!; Permissions = null!; }
        public Admin(Username username, Permissions permissions) : base(Guid.NewGuid())
        {
            Username = username ?? throw new DomainArgumentNullException(nameof(username));
            Permissions = permissions ?? throw new DomainArgumentNullException(nameof(permissions));
        }

        public override string ToString() => $"{Username.Value} (Админ)";

        // Навигационные коллекции
        private ICollection<Movie> CreatedMovies = new List<Movie>();
        public IReadOnlyCollection<Movie> createdMovies  => CreatedMovies.ToList().AsReadOnly();
        private ICollection<Rating> DeletedRatings = new List<Rating>();
        public IReadOnlyCollection<Rating> deletedRatings =>  DeletedRatings.ToList().AsReadOnly();

        //public void UpdateUsername(Username newUsername) => Username = newUsername;
        //public void UpdatePasswordHash(PasswordHash newHash) => PasswordHash = newHash;

        // Обновить права, false если новое значение совпадает с текущим
        public bool UpdatePermissions(Permissions newPermissions)
        {
            if (newPermissions == null) throw new DomainArgumentNullException(nameof(newPermissions));
            if (Permissions == newPermissions) return false;
            Permissions = newPermissions;
            return true;
        }

        // Проверить наличие права
        public bool HasPermission(string permission)
        {
            if (string.IsNullOrWhiteSpace(permission)) return false;
            return Permissions.Value.Split(',').Select(p => p.Trim()).Contains(permission);
        }

        // Создать новый фильм
        public Movie CreateMovie(
           Title title,
           Description? description = null,
           ReleaseYear? releaseYear = null,
           Genre? genre = null,
           PosterUrl? posterUrl = null)
        {
            if (title is null) throw new DomainArgumentNullException(nameof(title));

            if (CreatedMovies.Any(m => !m.IsDeleted && m.Title == title))
                throw new DuplicateMovieTitleException(this, title);

            var movie = new Movie(title, this, description, releaseYear, genre, posterUrl);
            CreatedMovies.Add(movie);
            return movie;
        }

        // Обновить информацию о фильме
        public Movie UpdateMovie(
            Movie movie,
            Title newTitle,
            Description? newDescription,
            ReleaseYear? newReleaseYear,
            Genre? newGenre,
            PosterUrl? newPosterUrl)
        {
            if (movie is null) throw new DomainArgumentNullException(nameof(movie));
            if (newTitle is null) throw new DomainArgumentNullException(nameof(newTitle));

            if (!CreatedMovies.Contains(movie))
                throw new MovieNotOwnedByAdminException(this, movie);
            if (movie.IsDeleted)
                throw new MovieAlreadyDeletedException(movie);

            movie.UpdateInfo(newTitle, newDescription, newReleaseYear, newGenre, newPosterUrl);
            return movie;
        }

        // Удалить фильм
        public bool DeleteMovie(Movie movie)
        {
            if (movie is null) throw new DomainArgumentNullException(nameof(movie));

            if (!CreatedMovies.Contains(movie))
                throw new MovieNotOwnedByAdminException(this, movie);
            if (movie.IsDeleted)
                throw new MovieAlreadyDeletedException(movie);

            movie.MarkAsDeleted();
            return true;
        }

        // Скрыть рецензию
        public void HideReview(Review review)
        {
            if (review is null) throw new DomainArgumentNullException(nameof(review));
            if (!review.IsActive)
                throw new ReviewAlreadyHiddenException(review);
            review.Hide(this);
        }

        // Восстановить рецензию
        public void UnhideReview(Review review)
        {
            if (review is null) throw new DomainArgumentNullException(nameof(review));
            if (review.IsActive)
                throw new ReviewNotHiddenException(review);
            review.Unhide();
        }

        // Удалить оценку
        public bool DeleteRating(Rating rating)
        {
            if (rating is null) throw new DomainArgumentNullException(nameof(rating));

            if (DeletedRatings.Contains(rating))
                throw new RatingAlreadyInDeletedListException(rating);

            rating.Delete(this);
            DeletedRatings.Add(rating);
            return true;
        }
    }
}