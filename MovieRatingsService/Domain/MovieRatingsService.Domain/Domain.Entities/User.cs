using Domain.ValueObject;
using Domain.ValueObject.Exceptions;
using MovieRatingsService.Domain.Base;


namespace MovieRatingsService.Domain.Domain.Entities { 

// Пользователь.Может оценивать фильмы и писать рецензии.
    public class User : Entity<Guid>
    {
        public Guid Id { get; private set; }
        public Username Username { get; private set; }

        public User(Username username):base(Guid.NewGuid())
        {
            Id = Guid.NewGuid();
            Username = username ?? throw new DomainArgumentNullException(nameof(username));
        }

        public void UpdateUsername(Username newUsername)
        {
            Username = newUsername ?? throw new DomainArgumentNullException(nameof(newUsername));
        }

        public override string ToString() => $"{Username.Value} (ID: {Id})";

        // Коллекции для навигации
        public virtual ICollection<Rating> Ratings { get; private set; } = new List<Rating>();
        public virtual ICollection<Review> Reviews { get; private set; } = new List<Review>();
    }
}