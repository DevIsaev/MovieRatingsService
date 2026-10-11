using MovieRatingsService.Application.Models.Base;

namespace MovieRatingsService.Application.Models.Review
{
    public record class CreateReviewModel(Guid UserId, Guid MovieId, string Content) : ICreateModel;
}
