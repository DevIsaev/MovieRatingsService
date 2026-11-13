using MovieRatingsService.Application.Service.Abstractions;
using MovieRatingsService.Domain.Repositories.Abstractions;

namespace MovieRatingsService.Application.Service
{
    //модерация - действия администратора над чужими рецензиями и оценками
    public class ModerationApplicationService(
        IAdminRepository admins,
        IReviewRepository reviews,
        IRatingRepository ratings) : IModerationApplicationService
    {
        public async Task<bool> HideReviewAsync(Guid adminId, Guid reviewId, CancellationToken cancellationToken = default)
        {
            var admin = await admins.GetAdminWithPermissionAsync(adminId, AdminPermissions.ModerateReviews, cancellationToken);
            var review = await reviews.GetByIdAsync(reviewId, cancellationToken);
            if (review is null)
                return false;

            admin.HideReview(review);
            return await reviews.UpdateAsync(review, cancellationToken);
        }

        public async Task<bool> UnhideReviewAsync(Guid adminId, Guid reviewId, CancellationToken cancellationToken = default)
        {
            var admin = await admins.GetAdminWithPermissionAsync(adminId, AdminPermissions.ModerateReviews, cancellationToken);
            var review = await reviews.GetByIdAsync(reviewId, cancellationToken);
            if (review is null)
                return false;

            admin.UnhideReview(review);
            return await reviews.UpdateAsync(review, cancellationToken);
        }

        public async Task<bool> DeleteRatingAsync(Guid adminId, Guid ratingId, CancellationToken cancellationToken = default)
        {
            var admin = await admins.GetAdminWithPermissionAsync(adminId, AdminPermissions.ModerateReviews, cancellationToken);
            var rating = await ratings.GetByIdAsync(ratingId, cancellationToken);
            if (rating is null)
                return false;

            admin.DeleteRating(rating);
            return await ratings.UpdateAsync(rating, cancellationToken);
        }
    }
}
