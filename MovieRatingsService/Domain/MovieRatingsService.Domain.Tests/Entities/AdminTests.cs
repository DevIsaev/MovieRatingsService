using Domain.ValueObject.Exceptions;
using Domain.ValueObject;
using MovieRatingsService.Domain.Domain.Entities;
using MovieRatingsService.Domain.Domain.Enums;

namespace MovieRatingsService.Domain.Tests.Entities
{
    public class AdminTests
    {
        private readonly Admin _admin = TestData.NewAdmin();
        private readonly User _alice = TestData.NewUser("alice");

        //Наследник чтобы вызвать protected-конструктор с явным Id
        private sealed class TestAdmin(Guid id, Username username, Permissions permissions)
            : Admin(id, username, permissions);

        //Конструкторы

        [Fact]
        public void Constructor_ValidArguments_SetsPropertiesAndGeneratesId()
        {
            var admin = new Admin(new Username("root"), new Permissions("a,b"));

            Assert.Equal("root", admin.Username.Value);
            Assert.Equal("a,b", admin.Permissions.Value);
            Assert.NotEqual(Guid.Empty, admin.Id);
        }

        [Fact]
        public void Constructor_NullUsername_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => new Admin(null!, new Permissions("a")));
        }

        [Fact]
        public void Constructor_NullPermissions_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => new Admin(new Username("root"), null!));
        }

        [Fact]
        public void ProtectedConstructor_WithId_UsesGivenId()
        {
            var id = Guid.NewGuid();

            var admin = new TestAdmin(id, new Username("root"), new Permissions("a"));

            Assert.Equal(id, admin.Id);
        }

        [Fact]
        public void ProtectedConstructor_NullUsername_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(
                () => new TestAdmin(Guid.NewGuid(), null!, new Permissions("a")));
        }

        [Fact]
        public void ProtectedConstructor_NullPermissions_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(
                () => new TestAdmin(Guid.NewGuid(), new Username("root"), null!));
        }

        [Fact]
        public void ToString_ContainsUsername()
        {
            Assert.Equal("root (Админ)", _admin.ToString());
        }

        //Права

        [Fact]
        public void UpdatePermissions_NewValue_ChangesAndReturnsTrue()
        {
            var changed = _admin.UpdatePermissions(new Permissions("only_one"));

            Assert.True(changed);
            Assert.Equal("only_one", _admin.Permissions.Value);
        }

        [Fact]
        public void UpdatePermissions_SameValue_ReturnsFalse()
        {
            var changed = _admin.UpdatePermissions(new Permissions("manage_movies,moderate_reviews"));

            Assert.False(changed);
        }

        [Fact]
        public void UpdatePermissions_Null_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => _admin.UpdatePermissions(null!));
        }

        [Theory]
        [InlineData("a,b", "a", true)]
        [InlineData("a,b", "b", true)]
        [InlineData("a, b", "b", true)]
        [InlineData("a,b", "c", false)]
        [InlineData("manage", "Manage", false)]
        [InlineData("a,b", "", false)]
        [InlineData("a,b", "   ", false)]
        [InlineData("a,b", null, false)]
        public void HasPermission_ReturnsExpectedResult(string permissions, string? permission, bool expected)
        {
            var admin = TestData.NewAdmin(permissions: permissions);

            Assert.Equal(expected, admin.HasPermission(permission!));
        }

        //CreateMovie

        [Fact]
        public void CreateMovie_ValidTitle_ReturnsMovieOwnedByAdmin()
        {
            var movie = _admin.CreateMovie(new Title("Inception"));

            Assert.Same(_admin, movie.CreatedByAdmin);
            Assert.Contains(movie, _admin.createdMovies);
        }

        [Fact]
        public void CreateMovie_WithOptionalFields_PassesThemToMovie()
        {
            var movie = _admin.CreateMovie(new Title("Inception"), new Description("d"),
                new ReleaseYear(2010), new Genre("Sci-Fi"), new PosterUrl("https://example.com/p.jpg"));

            Assert.Equal("d", movie.Description!.Value);
            Assert.Equal(2010, movie.ReleaseYear!.Value);
            Assert.Equal("Sci-Fi", movie.Genre!.Value);
            Assert.Equal("https://example.com/p.jpg", movie.PosterUrl!.Value);
        }

        [Fact]
        public void CreateMovie_NullTitle_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => _admin.CreateMovie(null!));
        }

        [Fact]
        public void CreateMovie_DuplicateTitle_ThrowsDuplicateMovieTitleException()
        {
            _admin.CreateMovie(new Title("Inception"));

            var ex = Assert.Throws<DuplicateMovieTitleException>(() => _admin.CreateMovie(new Title("Inception")));

            Assert.Same(_admin, ex.Admin);
            Assert.Equal("Inception", ex.Title.Value);
        }

        [Fact]
        public void CreateMovie_SameTitleAsDeletedMovie_Succeeds()
        {
            var first = _admin.CreateMovie(new Title("Inception"));
            _admin.DeleteMovie(first);

            var second = _admin.CreateMovie(new Title("Inception"));

            Assert.NotSame(first, second);
        }

        [Fact]
        public void CreateMovie_SameTitleFromAnotherAdmin_Succeeds()
        {
            _admin.CreateMovie(new Title("Inception"));

            var movie = TestData.NewAdmin("other").CreateMovie(new Title("Inception"));

            Assert.Equal("Inception", movie.Title.Value);
        }

        [Fact]
        public void CreatedMovies_ReturnsSnapshot_NotAffectedByLaterChanges()
        {
            var before = _admin.createdMovies;

            _admin.CreateMovie(new Title("Inception"));

            Assert.Empty(before);
            Assert.Single(_admin.createdMovies);
        }

        //UpdateMovie

        [Fact]
        public void UpdateMovie_OwnMovie_UpdatesAndReturnsSameMovie()
        {
            var movie = _admin.CreateMovie(new Title("Inception"));

            var result = _admin.UpdateMovie(movie, new Title("Tenet"), new Description("d"),
                new ReleaseYear(2020), new Genre("Action"), new PosterUrl("https://example.com/t.jpg"));

            Assert.Same(movie, result);
            Assert.Equal("Tenet", movie.Title.Value);
            Assert.Equal(2020, movie.ReleaseYear!.Value);
        }

        [Fact]
        public void UpdateMovie_NullMovie_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(
                () => _admin.UpdateMovie(null!, new Title("Tenet"), null, null, null, null));
        }

        [Fact]
        public void UpdateMovie_NullTitle_ThrowsDomainArgumentNullException()
        {
            var movie = _admin.CreateMovie(new Title("Inception"));

            Assert.Throws<DomainArgumentNullException>(() => _admin.UpdateMovie(movie, null!, null, null, null, null));
        }

        [Fact]
        public void UpdateMovie_MovieOfAnotherAdmin_ThrowsMovieNotOwnedByAdminException()
        {
            var foreign = TestData.NewMovie(TestData.NewAdmin("other"));

            var ex = Assert.Throws<MovieNotOwnedByAdminException>(
                () => _admin.UpdateMovie(foreign, new Title("Tenet"), null, null, null, null));

            Assert.Same(_admin, ex.Admin);
            Assert.Same(foreign, ex.Movie);
        }

        [Fact]
        public void UpdateMovie_DeletedMovie_ThrowsMovieAlreadyDeletedException()
        {
            var movie = _admin.CreateMovie(new Title("Inception"));
            _admin.DeleteMovie(movie);

            Assert.Throws<MovieAlreadyDeletedException>(
                () => _admin.UpdateMovie(movie, new Title("Tenet"), null, null, null, null));
        }

        //DeleteMovie

        [Fact]
        public void DeleteMovie_OwnMovieWithoutDependencies_ReturnsTrueAndMarksDeleted()
        {
            var movie = _admin.CreateMovie(new Title("Inception"));

            var result = _admin.DeleteMovie(movie);

            Assert.True(result);
            Assert.True(movie.IsDeleted);
        }

        [Fact]
        public void DeleteMovie_NullMovie_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => _admin.DeleteMovie(null!));
        }

        [Fact]
        public void DeleteMovie_MovieOfAnotherAdmin_ThrowsMovieNotOwnedByAdminException()
        {
            var foreign = TestData.NewMovie(TestData.NewAdmin("other"));

            Assert.Throws<MovieNotOwnedByAdminException>(() => _admin.DeleteMovie(foreign));
            Assert.False(foreign.IsDeleted);
        }

        [Fact]
        public void DeleteMovie_AlreadyDeleted_ThrowsMovieAlreadyDeletedException()
        {
            var movie = _admin.CreateMovie(new Title("Inception"));
            _admin.DeleteMovie(movie);

            Assert.Throws<MovieAlreadyDeletedException>(() => _admin.DeleteMovie(movie));
        }

        [Fact]
        public void DeleteMovie_WithActiveRating_ThrowsMovieHasDependenciesException()
        {
            var movie = _admin.CreateMovie(new Title("Inception"));
            movie.AddRating(_alice, new Score(5));

            Assert.Throws<MovieHasDependenciesException>(() => _admin.DeleteMovie(movie));
            Assert.False(movie.IsDeleted);
        }

        [Fact]
        public void DeleteMovie_AfterRatingWasDeleted_Succeeds()
        {
            var movie = _admin.CreateMovie(new Title("Inception"));
            _admin.DeleteRating(movie.AddRating(_alice, new Score(5)));

            Assert.True(_admin.DeleteMovie(movie));
        }

        //Модерация рецензий

        [Fact]
        public void HideReview_ActiveReview_HidesIt()
        {
            var review = TestData.NewMovie(_admin).AddReview(_alice, new ReviewContent("text"));

            _admin.HideReview(review);

            Assert.Equal(ReviewStatus.Hidden, review.Status);
            Assert.Same(_admin, review.HiddenByAdmin);
        }

        [Fact]
        public void HideReview_Null_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => _admin.HideReview(null!));
        }

        [Fact]
        public void HideReview_AlreadyHidden_ThrowsReviewAlreadyHiddenException()
        {
            var review = TestData.NewMovie(_admin).AddReview(_alice, new ReviewContent("text"));
            _admin.HideReview(review);

            Assert.Throws<ReviewAlreadyHiddenException>(() => _admin.HideReview(review));
        }

        [Fact]
        public void UnhideReview_HiddenReview_RestoresIt()
        {
            var review = TestData.NewMovie(_admin).AddReview(_alice, new ReviewContent("text"));
            _admin.HideReview(review);

            _admin.UnhideReview(review);

            Assert.True(review.IsActive);
        }

        [Fact]
        public void UnhideReview_Null_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => _admin.UnhideReview(null!));
        }

        [Fact]
        public void UnhideReview_ActiveReview_ThrowsReviewNotHiddenException()
        {
            var review = TestData.NewMovie(_admin).AddReview(_alice, new ReviewContent("text"));

            Assert.Throws<ReviewNotHiddenException>(() => _admin.UnhideReview(review));
        }

        //Модерация оценок

        [Fact]
        public void DeleteRating_ActiveRating_DeletesAndRemembersIt()
        {
            var rating = TestData.NewMovie(_admin).AddRating(_alice, new Score(5));

            var result = _admin.DeleteRating(rating);

            Assert.True(result);
            Assert.False(rating.IsActive);
            Assert.Same(_admin, rating.DeletedByAdmin);
            Assert.Contains(rating, _admin.deletedRatings);
        }

        [Fact]
        public void DeleteRating_Null_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => _admin.DeleteRating(null!));
        }

        [Fact]
        public void DeleteRating_SameAdminTwice_ThrowsRatingAlreadyInDeletedListException()
        {
            var rating = TestData.NewMovie(_admin).AddRating(_alice, new Score(5));
            _admin.DeleteRating(rating);

            var ex = Assert.Throws<RatingAlreadyInDeletedListException>(() => _admin.DeleteRating(rating));

            Assert.Same(rating, ex.Rating);
        }

        [Fact]
        public void DeleteRating_AlreadyDeletedByAnotherAdmin_ThrowsRatingAlreadyDeletedException()
        {
            var rating = TestData.NewMovie(_admin).AddRating(_alice, new Score(5));
            TestData.NewAdmin("other").DeleteRating(rating);

            Assert.Throws<RatingAlreadyDeletedException>(() => _admin.DeleteRating(rating));
            Assert.Empty(_admin.deletedRatings);
        }
    }
}
