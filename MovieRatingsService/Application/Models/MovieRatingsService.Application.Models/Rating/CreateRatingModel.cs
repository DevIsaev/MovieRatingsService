using MovieRatingsService.Application.Models.Base;

namespace MovieRatingsService.Application.Models.Rating
{
    public record class CreateRatingModel(Guid UserId, Guid MovieId, int Score) : ICreateModel;
}
