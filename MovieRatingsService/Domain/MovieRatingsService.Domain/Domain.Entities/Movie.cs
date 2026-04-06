using Domain.ValueObject;
using Domain.ValueObject.Exceptions;

namespace MovieRatingsService.Domain.Domain.Entities
{
    // Фильм в каталоге.
    public class Movie
    {
        public Guid Id { get; private set; }
        public Title Title { get; private set; }
        public string? Description { get; private set; }
        public int? ReleaseYear { get; private set; }
        public string? Genre { get; private set; }
        public string? PosterUrl { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Admin CreatedByAdmin { get; private set; }
        public bool IsDeleted { get; private set; }

        public Movie(Title title, Admin createdByAdmin, string? description = null,
                     int? releaseYear = null, string? genre = null, string? posterUrl = null)
        {
            Id = Guid.NewGuid();
            Title = title ?? throw new DomainArgumentNullException(nameof(title));
            CreatedByAdmin = createdByAdmin ?? throw new DomainArgumentNullException(nameof(createdByAdmin));
            CreatedAt = DateTime.UtcNow;
            Description = description;
            ReleaseYear = releaseYear;
            Genre = genre;
            PosterUrl = posterUrl;
            IsDeleted = false;
        }

        public void UpdateInfo(Title newTitle, string? newDescription, int? newReleaseYear,
                               string? newGenre, string? newPosterUrl)
        {
            Title = newTitle ?? throw new DomainArgumentNullException(nameof(newTitle));
            Description = newDescription;
            ReleaseYear = newReleaseYear;
            Genre = newGenre;
            PosterUrl = newPosterUrl;
        }

        public override string ToString() => $"{Title.Value} ({ReleaseYear})";

        // Навигационные коллекции
        public virtual ICollection<Rating> Ratings { get; private set; } = new List<Rating>();
        public virtual ICollection<Review> Reviews { get; private set; } = new List<Review>();

        // Отметка об удалении
        public void MarkAsDeleted()
        {
            if (Ratings.Any(r => r.IsActive) || Reviews.Any(r => r.IsActive))
                throw new MovieHasDependenciesException(Id);
            IsDeleted = true;
        }

        // Добавить оценку фильму от пользователя.
        public Rating AddRating(User user, Score score)
        {
            if (user == null) throw new DomainArgumentNullException(nameof(user));
            if (score == null) throw new DomainArgumentNullException(nameof(score));
            if (IsDeleted) throw new DomainOperationException("Невозможно оценить удалённый фильм.");

            if (Ratings.Any(r => r.User.Id == user.Id && r.IsActive))
                throw new DuplicateRatingException(user.Id, Id);

            var rating = new Rating(user, this, score);
            Ratings.Add(rating);
            return rating;
        }

        // Обновить оценку.
        public Rating UpdateRating(User user, Score newScore)
        {
            var rating = Ratings.FirstOrDefault(r => r.User.Id == user.Id && r.IsActive);
            if (rating == null)
                throw new EntityNotFoundException(nameof(Rating), $"user={user.Id}, movie={Id}");

            rating.UpdateScore(newScore);
            return rating;
        }

        // Удалить оценку (администратор).
        public void DeleteRating(Admin admin, User user)
        {
            var rating = Ratings.FirstOrDefault(r => r.User.Id == user.Id && r.IsActive);
            if (rating == null)
                throw new EntityNotFoundException(nameof(Rating), $"user={user.Id}, movie={Id}");

            rating.Delete(admin);
        }

        // Получить средний рейтинг.
        public double GetAverageScore()
        {
            var activeRatings = Ratings.Where(r => r.IsActive).ToList();
            if (!activeRatings.Any()) return 0;
            return activeRatings.Average(r => r.Score.Value);
        }

        // Получить количество активных оценок.
        public int GetRatingsCount() => Ratings.Count(r => r.IsActive);


        // Добавить рецензию.
        public Review AddReview(User user, ReviewContent content)
        {
            if (user == null) throw new DomainArgumentNullException(nameof(user));
            if (content == null) throw new DomainArgumentNullException(nameof(content));
            if (IsDeleted) throw new DomainOperationException("Невозможно оставить рецензию на удалённый фильм.");

            if (Reviews.Any(r => r.User.Id == user.Id && r.IsActive))
                throw new DuplicateReviewException(user.Id, Id);

            var review = new Review(user, this, content);
            Reviews.Add(review);
            return review;
        }
        // Обновить рецензию.
        public Review UpdateReview(User user, ReviewContent newContent)
        {
            var review = Reviews.FirstOrDefault(r => r.User.Id == user.Id && r.IsActive);
            if (review == null)
                throw new EntityNotFoundException(nameof(Review), $"user={user.Id}, movie={Id}");

            review.UpdateContent(newContent);
            return review;
        }

        // Скрыть рецензию (администратор).
        public void HideReview(Admin admin, User user)
        {
            var review = Reviews.FirstOrDefault(r => r.User.Id == user.Id && r.IsActive);
            if (review == null)
                throw new EntityNotFoundException(nameof(Review), $"user={user.Id}, movie={Id}");

            review.Hide(admin);
        }
    }
}
