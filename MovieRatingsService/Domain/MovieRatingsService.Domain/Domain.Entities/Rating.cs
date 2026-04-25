using Domain.ValueObject;
using Domain.ValueObject.Exceptions;
using MovieRatingsService.Domain.Base;

namespace MovieRatingsService.Domain.Domain.Entities
{
    // Оценка фильма пользователем
    public class Rating : Entity<Guid>
    {
        public User User { get; private set; }
        public Movie Movie { get; private set; }
        public Score Score { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }
        public Admin? DeletedByAdmin { get; private set; }

        protected Rating() : base() { User = null!; Movie = null!; Score = null!; }

        public Rating(User user, Movie movie, Score score) : base(Guid.NewGuid())
        {
            User = user ?? throw new DomainArgumentNullException(nameof(user));
            Movie = movie ?? throw new DomainArgumentNullException(nameof(movie));
            Score = score ?? throw new DomainArgumentNullException(nameof(score));
            CreatedAt = DateTime.UtcNow;
        }

        public bool IsActive => !DeletedAt.HasValue;

        public override string ToString() =>
            $"Рейтинг {Score.Value} фильма \"{Movie.Title.Value}\" от {User.Username.Value}";

        public void UpdateScore(Score newScore)
        {
            Score = newScore ?? throw new DomainArgumentNullException(nameof(newScore));
            UpdatedAt = DateTime.UtcNow;
        }

        public void Delete(Admin admin)
        {
            if (!IsActive)
                throw new RatingAlreadyDeletedException(this);
            DeletedByAdmin = admin ?? throw new DomainArgumentNullException(nameof(admin));
            DeletedAt = DateTime.UtcNow;
        }
    }
}