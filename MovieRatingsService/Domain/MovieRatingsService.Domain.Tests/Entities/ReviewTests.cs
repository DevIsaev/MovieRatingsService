using Domain.ValueObject.Exceptions;
using Domain.ValueObject;
using MovieRatingsService.Domain.Domain.Entities;
using MovieRatingsService.Domain.Domain.Enums;

namespace MovieRatingsService.Domain.Tests.Entities
{
    public class ReviewTests
    {
        private readonly User _user = TestData.NewUser();
        private readonly Movie _movie = TestData.NewMovie();

        private Review NewReview(string text = "Отличный фильм") => new(_user, _movie, new ReviewContent(text));

        [Fact]
        public void Constructor_ValidArguments_CreatesActiveReview()
        {
            var before = DateTime.UtcNow;

            var review = NewReview("Отличный фильм");

            Assert.Same(_user, review.User);
            Assert.Same(_movie, review.Movie);
            Assert.Equal("Отличный фильм", review.Content.Value);
            Assert.Equal(ReviewStatus.Active, review.Status);
            Assert.True(review.IsActive);
            Assert.Null(review.UpdatedAt);
            Assert.Null(review.HiddenByAdmin);
            Assert.Null(review.HiddenAt);
            Assert.InRange(review.CreatedAt, before, DateTime.UtcNow);
        }

        [Fact]
        public void Constructor_NullUser_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => new Review(null!, _movie, new ReviewContent("text")));
        }

        [Fact]
        public void Constructor_NullMovie_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => new Review(_user, null!, new ReviewContent("text")));
        }

        [Fact]
        public void Constructor_NullContent_ThrowsDomainArgumentNullException()
        {
            Assert.Throws<DomainArgumentNullException>(() => new Review(_user, _movie, null!));
        }

        [Fact]
        public void UpdateContent_NewContent_ChangesContentAndSetsUpdatedAt()
        {
            var review = NewReview();

            review.UpdateContent(new ReviewContent("Новый текст"));

            Assert.Equal("Новый текст", review.Content.Value);
            Assert.NotNull(review.UpdatedAt);
        }

        [Fact]
        public void UpdateContent_Null_ThrowsAndKeepsOldContent()
        {
            var review = NewReview("Старый текст");

            Assert.Throws<DomainArgumentNullException>(() => review.UpdateContent(null!));

            Assert.Equal("Старый текст", review.Content.Value);
        }

        [Fact]
        public void Hide_ActiveReview_HidesAndRemembersAdmin()
        {
            var admin = TestData.NewAdmin();
            var review = NewReview();

            review.Hide(admin);

            Assert.Equal(ReviewStatus.Hidden, review.Status);
            Assert.False(review.IsActive);
            Assert.Same(admin, review.HiddenByAdmin);
            Assert.NotNull(review.HiddenAt);
        }

        [Fact]
        public void Hide_AlreadyHidden_ThrowsReviewAlreadyHiddenException()
        {
            var review = NewReview();
            review.Hide(TestData.NewAdmin());

            var ex = Assert.Throws<ReviewAlreadyHiddenException>(() => review.Hide(TestData.NewAdmin()));

            Assert.Same(review, ex.Review);
        }

        [Fact]
        public void Hide_NullAdmin_ThrowsAndReviewStaysActive()
        {
            var review = NewReview();

            Assert.Throws<DomainArgumentNullException>(() => review.Hide(null!));

            Assert.Equal(ReviewStatus.Active, review.Status);
            Assert.True(review.IsActive);
        }

        [Fact]
        public void Unhide_HiddenReview_RestoresActiveAndClearsHiddenInfo()
        {
            var review = NewReview();
            review.Hide(TestData.NewAdmin());

            review.Unhide();

            Assert.Equal(ReviewStatus.Active, review.Status);
            Assert.Null(review.HiddenByAdmin);
            Assert.Null(review.HiddenAt);
        }

        [Fact]
        public void Unhide_ActiveReview_ThrowsReviewNotHiddenException()
        {
            var review = NewReview();

            var ex = Assert.Throws<ReviewNotHiddenException>(() => review.Unhide());

            Assert.Same(review, ex.Review);
        }

        [Fact]
        public void ToString_ShortContent_ContainsWholeText()
        {
            Assert.Equal("Отзыв от alice на фильм \"Inception\" (Great...)", NewReview("Great").ToString());
        }

        [Fact]
        public void ToString_LongContent_TruncatesToTwentyCharacters()
        {
            var review = NewReview(new string('x', 30));

            Assert.Equal($"Отзыв от alice на фильм \"Inception\" ({new string('x', 20)}...)", review.ToString());
        }
    }
}
