using Domain.ValueObject;
using Domain.ValueObject.Exceptions;

namespace MovieRatingsService.Domain.Domain.Entities
{
    // Оценка фильма пользователем.
    public class Rating
    {
        public Guid Id { get; private set; }
        public User User { get; private set; }
        public Movie Movie { get; private set; }
        public Score Score { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }
        public Admin? DeletedByAdmin { get; private set; }

        public Rating(User user, Movie movie, Score score)
        {
            Id = Guid.NewGuid();
            User = user ?? throw new DomainArgumentNullException(nameof(user));
            Movie = movie ?? throw new DomainArgumentNullException(nameof(movie));
            Score = score ?? throw new DomainArgumentNullException(nameof(score));
            CreatedAt = DateTime.UtcNow;
        }

        public bool IsActive => !DeletedAt.HasValue;

        public override string ToString() => $"Рейтинг {Score.Value} фильма {Movie.Title.Value} от {User.Username.Value}";

        public void UpdateScore(Score newScore)
        {
            Score = newScore ?? throw new DomainArgumentNullException(nameof(newScore));
            UpdatedAt = DateTime.UtcNow;
        }

        public void Delete(Admin admin)
        {
            if (DeletedAt.HasValue)
                throw new RatingAlreadyDeletedException(Id);
            DeletedAt = DateTime.UtcNow;
            DeletedByAdmin = admin ?? throw new DomainArgumentNullException(nameof(admin));
        }
    }
}
