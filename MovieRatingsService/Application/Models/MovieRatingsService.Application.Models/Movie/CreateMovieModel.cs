using MovieRatingsService.Application.Models.Base;

namespace MovieRatingsService.Application.Models.Movie
{
    public record class CreateMovieModel(
        Guid AdminId,
        string Title,
        string? Description,
        int? ReleaseYear,
        string? Genre,
        string? PosterUrl) : ICreateModel;
}
