using MovieRatingsService.Application.Models.Review;

namespace MovieRatingsService.Application.Service.Abstractions
{
    public interface IReviewsApplicationService
    {
        Task<IEnumerable<ReviewModel>> GetReviewsByMovieAsync(Guid movieId, CancellationToken cancellationToken);
        Task<IEnumerable<ReviewModel>> GetReviewsByUserAsync(Guid userId, CancellationToken cancellationToken);
        Task<ReviewModel?> GetUserReviewAsync(Guid userId, Guid movieId, CancellationToken cancellationToken);
        Task<ReviewModel?> AddReviewAsync(CreateReviewModel model, CancellationToken cancellationToken);
        Task<bool> UpdateReviewAsync(UpdateReviewModel model, CancellationToken cancellationToken);
    }
}
