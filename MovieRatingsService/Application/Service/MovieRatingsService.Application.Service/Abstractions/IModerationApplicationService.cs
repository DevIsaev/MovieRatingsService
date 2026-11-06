namespace MovieRatingsService.Application.Service.Abstractions
{
    public interface IModerationApplicationService
    {
        Task<bool> HideReviewAsync(Guid adminId, Guid reviewId, CancellationToken cancellationToken);
        Task<bool> UnhideReviewAsync(Guid adminId, Guid reviewId, CancellationToken cancellationToken);
        Task<bool> DeleteRatingAsync(Guid adminId, Guid ratingId, CancellationToken cancellationToken);
    }
}
