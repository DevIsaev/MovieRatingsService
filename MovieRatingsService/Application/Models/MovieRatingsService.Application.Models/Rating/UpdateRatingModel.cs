namespace MovieRatingsService.Application.Models.Rating
{
    public record class UpdateRatingModel(Guid UserId, Guid MovieId, int Score);
}
