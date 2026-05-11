using Domain.ValueObject;
using Domain.ValueObject.Exceptions;
using MovieRatingsService.Domain.Base;


namespace MovieRatingsService.Domain.Domain.Entities
{
    // Фильм в каталоге
    public class Movie : Entity<Guid>
    {
        public Title Title { get; private set; }
        public Description? Description { get; private set; }
        public ReleaseYear? ReleaseYear { get; private set; }
        public Genre? Genre { get; private set; }
        public PosterUrl? PosterUrl { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Admin CreatedByAdmin { get; private set; }
        public bool IsDeleted { get; private set; }

        protected Movie() : base()
        {
            Title = null!;
            CreatedByAdmin = null!;
        }

        public Movie(
                    Title title,
                    Admin createdByAdmin,
                    Description? description = null,
                    ReleaseYear? releaseYear = null,
                    Genre? genre = null,
                    PosterUrl? posterUrl = null) : base(Guid.NewGuid())
        {
            Title = title ?? throw new DomainArgumentNullException(nameof(title));
            CreatedByAdmin = createdByAdmin ?? throw new DomainArgumentNullException(nameof(createdByAdmin));
            CreatedAt = DateTime.UtcNow;
            Description = description;
            ReleaseYear = releaseYear;
            Genre = genre;
            PosterUrl = posterUrl;
            IsDeleted = false;
        }

        public void UpdateInfo(
                    Title newTitle,
                    Description? newDescription,
                    ReleaseYear? newReleaseYear,
                    Genre? newGenre,
                    PosterUrl? newPosterUrl)
        {
            Title = newTitle ?? throw new DomainArgumentNullException(nameof(newTitle));
            Description = newDescription;
            ReleaseYear = newReleaseYear;
            Genre = newGenre;
            PosterUrl = newPosterUrl;
        }

        public override string ToString() => $"{Title.Value} ({ReleaseYear?.Value})";

        // Навигационные коллекции
        private ICollection<Rating> Ratings  = new List<Rating>();
        private ICollection<Review> Reviews  = new List<Review>();

        // Отметка об удалении
        public void MarkAsDeleted()
        {
            if (Ratings.Any(r => r.IsActive) || Reviews.Any(r => r.IsActive))
                throw new MovieHasDependenciesException(this);
            IsDeleted = true;
        }

        // Добавить оценку фильму от пользователя. Возвращает созданную оценку
        public Rating AddRating(User user, Score score)
        {
            if (user == null) throw new DomainArgumentNullException(nameof(user));
            if (score == null) throw new DomainArgumentNullException(nameof(score));
            if (IsDeleted) throw new MovieAlreadyDeletedException(this);

            if (Ratings.Any(r => r.User.Id == user.Id && r.IsActive))
                throw new DuplicateRatingException(user, this);

            var rating = new Rating(user, this, score);
            Ratings.Add(rating);
            return rating;
        }

        // Обновить оценку. Возвращает обновлённую оценку
        public Rating UpdateRating(User user, Score newScore)
        {
            if (user == null) throw new DomainArgumentNullException(nameof(user));
            if (newScore == null) throw new DomainArgumentNullException(nameof(newScore));

            var rating = Ratings.FirstOrDefault(r => r.User.Id == user.Id && r.IsActive);
            if (rating == null)
                throw new EntityNotFoundException(nameof(Rating), $"user={user.Id}, movie={Id}");

            rating.UpdateScore(newScore);
            return rating;
        }

        // Получить средний рейтинг
        public double GetAverageScore()
        {
            var activeRatings = Ratings.Where(r => r.IsActive).ToList();
            if (!activeRatings.Any()) return 0;
            return activeRatings.Average(r => r.Score.Value);
        }

        // Получить количество активных оценок
        public int GetRatingsCount() => Ratings.Count(r => r.IsActive);

        // Добавить рецензию. Возвращает созданную рецензию
        public Review AddReview(User user, ReviewContent content)
        {
            if (user == null) throw new DomainArgumentNullException(nameof(user));
            if (content == null) throw new DomainArgumentNullException(nameof(content));
            if (IsDeleted) throw new MovieAlreadyDeletedException(this);

            if (Reviews.Any(r => r.User.Id == user.Id && r.IsActive))
                throw new DuplicateReviewException(user, this);

            var review = new Review(user, this, content);
            Reviews.Add(review);
            return review;
        }

        // Обновить рецензию. Возвращает обновлённую рецензию
        public Review UpdateReview(User user, ReviewContent newContent)
        {
            if (user == null) throw new DomainArgumentNullException(nameof(user));
            if (newContent == null) throw new DomainArgumentNullException(nameof(newContent));

            var review = Reviews.FirstOrDefault(r => r.User.Id == user.Id && r.IsActive);
            if (review == null)
                throw new EntityNotFoundException(nameof(Review), $"user={user.Id}, movie={Id}");

            review.UpdateContent(newContent);
            return review;
        }
    }
}
