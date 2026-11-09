using Domain.ValueObject.Exceptions;
using Domain.ValueObject;
using MovieRatingsService.Domain.Domain.Entities;

namespace MovieRatingsService.Domain.Tests.Entities
{
    public class MovieTests
    {
        private readonly Admin _admin = TestData.NewAdmin();
        private readonly User _alice = TestData.NewUser("alice");
        private readonly User _bob = TestData.NewUser("bob");

        private Movie NewMovie() => TestData.NewMovie(_admin);

        //Конструктор и UpdateInfo

        [Fact]
        public void Constructor_OnlyRequiredArguments_SetsDefaults()
        {
            var before = DateTime.UtcNow;

            var movie = new Movie(new Title("Inception"), _admin);

            Assert.Equal("Inception", movie.Title.Value);
            Assert.Same(_admin, movie.CreatedByAdmin);
            Assert.NotEqual(Guid.Empty, movie.Id);
            Assert.False(movie.IsDeleted);
            Assert.Null(movie.Description);
            Assert.Null(movie.ReleaseYear);
            Assert.Null(movie.Genre);
            Assert.Null(movie.PosterUrl);
            Assert.InRange(movie.CreatedAt, before, DateTime.UtcNow);
        }

        [Fact]
        public void Constructor_AllArguments_SetsAllProperties()
        {
            var movie = new Movie(
                new Title("Inception"), _admin,
                new Description("Сны внутри снов"), new ReleaseYear(2010),
                new Genre("Sci-Fi"), new PosterUrl("https://example.com/p.jpg"));

            Assert.Equal("Сны внутри снов", movie.Description!.Value);
            Assert.Equal(2010, movie.ReleaseYear!.Value);
            Assert.Equal("Sci-Fi", movie.Genre!.Value);
            Assert.Equal("https://example.com/p.jpg", movie.PosterUrl!.Value);
        }

        [Fact]
        public void Constructor_NullTitle_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => new Movie(null!, _admin));
        }

        [Fact]
        public void Constructor_NullAdmin_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => new Movie(new Title("Inception"), null!));
        }

        [Fact]
        public void UpdateInfo_NewValues_ReplacesAllFields()
        {
            var movie = NewMovie();

            movie.UpdateInfo(new Title("Tenet"), new Description("d"), new ReleaseYear(2020),
                new Genre("Action"), new PosterUrl("https://example.com/t.jpg"));

            Assert.Equal("Tenet", movie.Title.Value);
            Assert.Equal("d", movie.Description!.Value);
            Assert.Equal(2020, movie.ReleaseYear!.Value);
            Assert.Equal("Action", movie.Genre!.Value);
            Assert.Equal("https://example.com/t.jpg", movie.PosterUrl!.Value);
        }

        [Fact]
        public void UpdateInfo_NullTitle_ThrowsAndKeepsOldTitle()
        {
            var movie = NewMovie();

            Assert.Throws<DomainArgumentNullException>(() => movie.UpdateInfo(null!, null, null, null, null));

            Assert.Equal("Inception", movie.Title.Value);
        }

        [Fact]
        public void ToString_WithReleaseYear_ContainsYear()
        {
            var movie = new Movie(new Title("Inception"), _admin, releaseYear: new ReleaseYear(2010));

            Assert.Equal("Inception (2010)", movie.ToString());
        }

        [Fact]
        public void ToString_WithoutReleaseYear_HasEmptyBrackets()
        {
            Assert.Equal("Inception ()", new Movie(new Title("Inception"), _admin).ToString());
        }

        //Оценки

        [Fact]
        public void AddRating_ValidArguments_ReturnsRatingAndCountsIt()
        {
            var movie = NewMovie();

            var rating = movie.AddRating(_alice, new Score(8));

            Assert.Same(_alice, rating.User);
            Assert.Same(movie, rating.Movie);
            Assert.Equal(8, rating.Score.Value);
            Assert.Equal(1, movie.GetRatingsCount());
        }

        [Fact]
        public void AddRating_NullUser_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => NewMovie().AddRating(null!, new Score(5)));
        }

        [Fact]
        public void AddRating_NullScore_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => NewMovie().AddRating(_alice, null!));
        }

        [Fact]
        public void AddRating_DeletedMovie_ThrowsMovieAlreadyDeletedException()
        {
            var movie = NewMovie();
            _admin.DeleteMovie(movie);

            var ex = Assert.Throws<MovieAlreadyDeletedException>(() => movie.AddRating(_alice, new Score(5)));

            Assert.Same(movie, ex.Movie);
        }

        [Fact]
        public void AddRating_SameUserTwice_ThrowsDuplicateRatingException()
        {
            var movie = NewMovie();
            movie.AddRating(_alice, new Score(5));

            var ex = Assert.Throws<DuplicateRatingException>(() => movie.AddRating(_alice, new Score(7)));

            Assert.Same(_alice, ex.User);
            Assert.Same(movie, ex.Movie);
        }

        [Fact]
        public void AddRating_AfterAdminDeletedPreviousRating_Succeeds()
        {
            var movie = NewMovie();
            _admin.DeleteRating(movie.AddRating(_alice, new Score(5)));

            movie.AddRating(_alice, new Score(9));

            Assert.Equal(1, movie.GetRatingsCount());
        }

        [Fact]
        public void UpdateRating_ExistingRating_ChangesScore()
        {
            var movie = NewMovie();
            movie.AddRating(_alice, new Score(5));

            var rating = movie.UpdateRating(_alice, new Score(9));

            Assert.Equal(9, rating.Score.Value);
            Assert.NotNull(rating.UpdatedAt);
        }

        [Fact]
        public void UpdateRating_NoRatingFromUser_ThrowsEntityNotFoundException()
        {
            var movie = NewMovie();
            movie.AddRating(_alice, new Score(5));

            Assert.Throws<EntityNotFoundException>(() => movie.UpdateRating(_bob, new Score(7)));
        }

        [Fact]
        public void UpdateRating_RatingDeletedByAdmin_ThrowsEntityNotFoundException()
        {
            var movie = NewMovie();
            _admin.DeleteRating(movie.AddRating(_alice, new Score(5)));

            Assert.Throws<EntityNotFoundException>(() => movie.UpdateRating(_alice, new Score(7)));
        }

        [Fact]
        public void UpdateRating_NullUser_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => NewMovie().UpdateRating(null!, new Score(5)));
        }

        [Fact]
        public void UpdateRating_NullScore_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => NewMovie().UpdateRating(_alice, null!));
        }

        [Fact]
        public void GetAverageScore_NoRatings_ReturnsZero()
        {
            Assert.Equal(0, NewMovie().GetAverageScore());
        }

        [Fact]
        public void GetAverageScore_TwoRatings_ReturnsArithmeticMean()
        {
            var movie = NewMovie();
            movie.AddRating(_alice, new Score(4));
            movie.AddRating(_bob, new Score(7));

            Assert.Equal(5.5, movie.GetAverageScore());
        }

        [Fact]
        public void GetAverageScore_IgnoresRatingsDeletedByAdmin()
        {
            var movie = NewMovie();
            movie.AddRating(_alice, new Score(4));
            _admin.DeleteRating(movie.AddRating(_bob, new Score(10)));

            Assert.Equal(4, movie.GetAverageScore());
        }

        [Fact]
        public void GetRatingsCount_CountsOnlyActiveRatings()
        {
            var movie = NewMovie();
            movie.AddRating(_alice, new Score(4));
            _admin.DeleteRating(movie.AddRating(_bob, new Score(10)));

            Assert.Equal(1, movie.GetRatingsCount());
        }

        //Рецензии

        [Fact]
        public void AddReview_ValidArguments_ReturnsReview()
        {
            var movie = NewMovie();

            var review = movie.AddReview(_alice, new ReviewContent("Класс"));

            Assert.Same(_alice, review.User);
            Assert.Same(movie, review.Movie);
            Assert.Equal("Класс", review.Content.Value);
        }

        [Fact]
        public void AddReview_NullUser_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => NewMovie().AddReview(null!, new ReviewContent("text")));
        }

        [Fact]
        public void AddReview_NullContent_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => NewMovie().AddReview(_alice, null!));
        }

        [Fact]
        public void AddReview_DeletedMovie_ThrowsMovieAlreadyDeletedException()
        {
            var movie = NewMovie();
            _admin.DeleteMovie(movie);

            Assert.Throws<MovieAlreadyDeletedException>(() => movie.AddReview(_alice, new ReviewContent("text")));
        }

        [Fact]
        public void AddReview_SameUserTwice_ThrowsDuplicateReviewException()
        {
            var movie = NewMovie();
            movie.AddReview(_alice, new ReviewContent("one"));

            var ex = Assert.Throws<DuplicateReviewException>(() => movie.AddReview(_alice, new ReviewContent("two")));

            Assert.Same(_alice, ex.User);
            Assert.Same(movie, ex.Movie);
        }

        [Fact]
        public void AddReview_AfterAdminHidPreviousReview_Succeeds()
        {
            var movie = NewMovie();
            _admin.HideReview(movie.AddReview(_alice, new ReviewContent("one")));

            var second = movie.AddReview(_alice, new ReviewContent("two"));

            Assert.True(second.IsActive);
        }

        [Fact]
        public void UpdateReview_ExistingReview_ChangesContent()
        {
            var movie = NewMovie();
            movie.AddReview(_alice, new ReviewContent("old"));

            var review = movie.UpdateReview(_alice, new ReviewContent("new"));

            Assert.Equal("new", review.Content.Value);
            Assert.NotNull(review.UpdatedAt);
        }

        [Fact]
        public void UpdateReview_NoReviewFromUser_ThrowsEntityNotFoundException()
        {
            Assert.Throws<EntityNotFoundException>(() => NewMovie().UpdateReview(_alice, new ReviewContent("text")));
        }

        [Fact]
        public void UpdateReview_ReviewHiddenByAdmin_ThrowsEntityNotFoundException()
        {
            var movie = NewMovie();
            _admin.HideReview(movie.AddReview(_alice, new ReviewContent("old")));

            Assert.Throws<EntityNotFoundException>(() => movie.UpdateReview(_alice, new ReviewContent("new")));
        }

        [Fact]
        public void UpdateReview_NullUser_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => NewMovie().UpdateReview(null!, new ReviewContent("text")));
        }

        [Fact]
        public void UpdateReview_NullContent_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => NewMovie().UpdateReview(_alice, null!));
        }

        //Удаление

        [Fact]
        public void MarkAsDeleted_NoDependencies_SetsIsDeleted()
        {
            var movie = NewMovie();

            movie.MarkAsDeleted();

            Assert.True(movie.IsDeleted);
        }

        [Fact]
        public void MarkAsDeleted_ActiveRating_ThrowsMovieHasDependenciesException()
        {
            var movie = NewMovie();
            movie.AddRating(_alice, new Score(5));

            var ex = Assert.Throws<MovieHasDependenciesException>(() => movie.MarkAsDeleted());

            Assert.Same(movie, ex.Movie);
            Assert.False(movie.IsDeleted);
        }

        [Fact]
        public void MarkAsDeleted_ActiveReview_ThrowsMovieHasDependenciesException()
        {
            var movie = NewMovie();
            movie.AddReview(_alice, new ReviewContent("text"));

            Assert.Throws<MovieHasDependenciesException>(() => movie.MarkAsDeleted());
            Assert.False(movie.IsDeleted);
        }

        [Fact]
        public void MarkAsDeleted_OnlyDeletedRatingsAndHiddenReviews_Succeeds()
        {
            var movie = NewMovie();
            _admin.DeleteRating(movie.AddRating(_alice, new Score(5)));
            _admin.HideReview(movie.AddReview(_alice, new ReviewContent("text")));

            movie.MarkAsDeleted();

            Assert.True(movie.IsDeleted);
        }
    }
}