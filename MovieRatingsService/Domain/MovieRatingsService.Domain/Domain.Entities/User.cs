using Domain.ValueObject;
using Domain.ValueObject.Exceptions;
using MovieRatingsService.Domain.Base;


namespace MovieRatingsService.Domain.Domain.Entities { 

// Пользователь. Может оценивать фильмы и писать рецензии
    public class User : Entity<Guid>
    {
        public Username Username { get; private set; }
        protected User() : base() { Username = null!; }

        public User(Username username) : base(Guid.NewGuid())
        {
            Username = username ?? throw new DomainArgumentNullException(nameof(username));
        }

        public bool UpdateUsername(Username newUsername)
        {
            if (newUsername is null) throw new DomainArgumentNullException(nameof(newUsername));
            if (Username == newUsername) return false;
            Username = newUsername;
            return true;
        }

        public override string ToString() => $"{Username.Value} (ID: {Id})";

        // Коллекции для навигации
        private ICollection<Rating> Ratings = new List<Rating>();
        private ICollection<Review> Reviews = new List<Review>();
    }
}