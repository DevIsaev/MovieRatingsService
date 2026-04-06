using Domain.ValueObject;
using Domain.ValueObject.Exceptions;
using MovieRatingsService.Domain.Domain.Enums;

namespace MovieRatingsService.Domain.Domain.Entities
{
    // Рецензия пользователя на фильм.
    public class Review
    {
        public Guid Id { get; private set; }
        public User User { get; private set; }
        public Movie Movie { get; private set; }
        public ReviewContent Content { get; private set; }
        public ReviewStatus Status { get; private set; } = ReviewStatus.Active;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Admin? HiddenByAdmin { get; private set; }
        public DateTime? HiddenAt { get; private set; }

        public Review(User user, Movie movie, ReviewContent content)
        {
            Id = Guid.NewGuid();
            User = user ?? throw new DomainArgumentNullException(nameof(user));
            Movie = movie ?? throw new DomainArgumentNullException(nameof(movie));
            Content = content ?? throw new DomainArgumentNullException(nameof(content));
            CreatedAt = DateTime.UtcNow;
        }

        public bool IsActive => Status == ReviewStatus.Active;

        public override string ToString() => $"Отзыв от {User.Username.Value} на фильм {Movie.Title.Value} ({Content.Value[..Math.Min(20, Content.Value.Length)]}...)";

        public void UpdateContent(ReviewContent newContent)
        {
            Content = newContent ?? throw new DomainArgumentNullException(nameof(newContent));
            UpdatedAt = DateTime.UtcNow;
        }

        public void Hide(Admin admin)
        {
            if (Status == ReviewStatus.Hidden)
                throw new ReviewAlreadyHiddenException(Id);
            Status = ReviewStatus.Hidden;
            HiddenByAdmin = admin ?? throw new DomainArgumentNullException(nameof(admin));
            HiddenAt = DateTime.UtcNow;
        }

        public void Unhide()
        {
            if (Status != ReviewStatus.Hidden)
                throw new DomainOperationException("Рецензия не скрыта.");
            Status = ReviewStatus.Active;
            HiddenByAdmin = null;
            HiddenAt = null;
        }
    }
}
