using MovieRatingsService.Application.Models.Movie;

namespace MovieRatingsService.Application.Service.Abstractions
{
    public interface IMoviesApplicationService
    {
        Task<IEnumerable<MovieModel>> GetMoviesAsync(CancellationToken cancellationToken);
        Task<MovieModel?> GetMovieByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<MovieModel>> SearchMoviesAsync(string title, CancellationToken cancellationToken);
        Task<IEnumerable<MovieModel>> GetMoviesByAdminAsync(Guid adminId, CancellationToken cancellationToken);
        Task<MovieModel?> CreateMovieAsync(CreateMovieModel model, CancellationToken cancellationToken);
        Task<bool> UpdateMovieAsync(UpdateMovieModel model, CancellationToken cancellationToken);
        Task<bool> DeleteMovieAsync(Guid adminId, Guid movieId, CancellationToken cancellationToken);
    }
}
