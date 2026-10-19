namespace MovieRatingsService.Application.Models.Movie
{
    public record class UpdateMovieModel(
        Guid Id,
        Guid AdminId,
        string Title,
        string? Description,
        int? ReleaseYear,
        string? Genre,
        string? PosterUrl);
}
