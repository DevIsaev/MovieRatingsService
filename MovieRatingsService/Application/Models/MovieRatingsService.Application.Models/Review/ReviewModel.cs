using MovieRatingsService.Application.Models.Base;

namespace MovieRatingsService.Application.Models.Review
{
    public record class ReviewModel(
        Guid Id, Guid UserId, Guid MovieId, string Content, string Status,
        DateTime CreatedAt, DateTime? UpdatedAt) : IModel<Guid>;
}
