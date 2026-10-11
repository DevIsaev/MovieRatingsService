namespace MovieRatingsService.Application.Models.Review
{
    public record class UpdateReviewModel(Guid UserId, Guid MovieId, string Content);
}
