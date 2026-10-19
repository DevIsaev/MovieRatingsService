using MovieRatingsService.Application.Models.Rating;

namespace MovieRatingsService.Application.Service.Abstractions
{
    public interface IRatingsApplicationService
    {
        Task<IEnumerable<RatingModel>> GetRatingsByMovieAsync(Guid movieId, CancellationToken cancellationToken);
        Task<IEnumerable<RatingModel>> GetRatingsByUserAsync(Guid userId, CancellationToken cancellationToken);
        Task<RatingModel?> GetUserRatingAsync(Guid userId, Guid movieId, CancellationToken cancellationToken);
        Task<RatingModel?> RateMovieAsync(CreateRatingModel model, CancellationToken cancellationToken);
        Task<bool> UpdateRatingAsync(UpdateRatingModel model, CancellationToken cancellationToken);
    }
}
