using MovieRatingsService.Application.Models.Base;

namespace MovieRatingsService.Application.Models.Rating
{
    public record class RatingModel(
        Guid Id, Guid UserId, Guid MovieId, int Score,
        DateTime CreatedAt, DateTime? UpdatedAt) : IModel<Guid>;
}
