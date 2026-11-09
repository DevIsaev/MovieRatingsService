using Domain.ValueObject.Exceptions;
using Domain.ValueObject;
using MovieRatingsService.Domain.Domain.Entities;

namespace MovieRatingsService.Domain.Tests.Entities
{
    public class RatingTests
    {
        private readonly User _user = TestData.NewUser();
        private readonly Movie _movie = TestData.NewMovie();

        [Fact]
        public void Constructor_ValidArguments_CreatesActiveRating()
        {
            var before = DateTime.UtcNow;

            var rating = new Rating(_user, _movie, new Score(8));

            Assert.Same(_user, rating.User);
            Assert.Same(_movie, rating.Movie);
            Assert.Equal(8, rating.Score.Value);
            Assert.True(rating.IsActive);
            Assert.Null(rating.UpdatedAt);
            Assert.Null(rating.DeletedAt);
            Assert.Null(rating.DeletedByAdmin);
            Assert.InRange(rating.CreatedAt, before, DateTime.UtcNow);
        }

        [Fact]
        public void Constructor_NullUser_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => new Rating(null!, _movie, new Score(5)));
        }

        [Fact]
        public void Constructor_NullMovie_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => new Rating(_user, null!, new Score(5)));
        }

        [Fact]
        public void Constructor_NullScore_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => new Rating(_user, _movie, null!));
        }

        [Fact]
        public void UpdateScore_NewScore_ChangesScoreAndSetsUpdatedAt()
        {
            var rating = new Rating(_user, _movie, new Score(5));

            rating.UpdateScore(new Score(9));

            Assert.Equal(9, rating.Score.Value);
            Assert.NotNull(rating.UpdatedAt);
        }

        [Fact]
        public void UpdateScore_Null_ThrowsAndKeepsOldScore()
        {
            var rating = new Rating(_user, _movie, new Score(5));

            Assert.Throws<DomainArgumentNullException>(() => rating.UpdateScore(null!));

            Assert.Equal(5, rating.Score.Value);
            Assert.Null(rating.UpdatedAt);
        }

        [Fact]
        public void Delete_ActiveRating_MarksDeletedByAdmin()
        {
            var admin = TestData.NewAdmin();
            var rating = new Rating(_user, _movie, new Score(5));

            rating.Delete(admin);

            Assert.False(rating.IsActive);
            Assert.NotNull(rating.DeletedAt);
            Assert.Same(admin, rating.DeletedByAdmin);
        }

        [Fact]
        public void Delete_NullAdmin_ThrowsAndRatingStaysActive()
        {
            var rating = new Rating(_user, _movie, new Score(5));

            Assert.Throws<DomainArgumentNullException>(() => rating.Delete(null!));

            Assert.True(rating.IsActive);
        }

        [Fact]
        public void Delete_AlreadyDeleted_ThrowsRatingAlreadyDeletedException()
        {
            var rating = new Rating(_user, _movie, new Score(5));
            rating.Delete(TestData.NewAdmin());

            var ex = Assert.Throws<RatingAlreadyDeletedException>(() => rating.Delete(TestData.NewAdmin()));

            Assert.Same(rating, ex.Rating);
        }

        [Fact]
        public void ToString_ContainsScoreMovieAndUser()
        {
            var rating = new Rating(_user, _movie, new Score(8));

            Assert.Equal("Рейтинг 8 фильма \"Inception\" от alice", rating.ToString());
        }
    }
}
