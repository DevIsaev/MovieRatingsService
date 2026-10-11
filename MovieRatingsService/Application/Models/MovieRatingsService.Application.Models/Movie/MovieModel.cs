using MovieRatingsService.Application.Models.Base;

namespace MovieRatingsService.Application.Models.Movie
{
    public record class MovieModel(
        Guid Id,
        string Title,
        string? Description,
        int? ReleaseYear,
        string? Genre,
        string? PosterUrl,
        DateTime CreatedAt,
        Guid CreatedByAdminId,
        bool IsDeleted,
        double AverageScore,
        int RatingsCount) : IModel<Guid>;
}
