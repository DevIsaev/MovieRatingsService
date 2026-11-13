using AutoMapper;
using Domain.ValueObject;
using MovieRatingsService.Application.Models.Movie;
using MovieRatingsService.Application.Service.Abstractions;
using MovieRatingsService.Domain.Repositories.Abstractions;

namespace MovieRatingsService.Application.Service
{
    public class MoviesApplicationService(
        IMovieRepository movies,
        IAdminRepository admins,
        IMapper mapper) : IMoviesApplicationService
    {
        public async Task<IEnumerable<MovieModel>> GetMoviesAsync(CancellationToken cancellationToken = default)
            => mapper.Map<IEnumerable<MovieModel>>(await movies.GetActiveMoviesAsync(cancellationToken));

        public async Task<MovieModel?> GetMovieByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var movie = await movies.GetByIdAsync(id, cancellationToken);
            return movie is null ? null : mapper.Map<MovieModel>(movie);
        }

        public async Task<IEnumerable<MovieModel>> SearchMoviesAsync(string title, CancellationToken cancellationToken = default)
            => mapper.Map<IEnumerable<MovieModel>>(await movies.SearchByTitleAsync(title.Trim(), cancellationToken));

        public async Task<IEnumerable<MovieModel>> GetMoviesByAdminAsync(Guid adminId, CancellationToken cancellationToken = default)
            => mapper.Map<IEnumerable<MovieModel>>(await movies.GetByAdminAsync(adminId, cancellationToken));

        public async Task<MovieModel?> CreateMovieAsync(CreateMovieModel model, CancellationToken cancellationToken = default)
        {
            var admin = await admins.GetAdminWithPermissionAsync(model.AdminId, AdminPermissions.ManageMovies, cancellationToken);

            //доменный метод проверяет дубликат названия среди фильмов этого администратора
            var movie = admin.CreateMovie(
                new Title(model.Title),
                ToDescription(model.Description),
                ToReleaseYear(model.ReleaseYear),
                ToGenre(model.Genre),
                ToPosterUrl(model.PosterUrl));

            var created = await movies.AddAsync(movie, cancellationToken);
            return created is null ? null : mapper.Map<MovieModel>(created);
        }

        public async Task<bool> UpdateMovieAsync(UpdateMovieModel model, CancellationToken cancellationToken = default)
        {
            //сначала администратор с его фильмами, потом фильм
            var admin = await admins.GetAdminWithPermissionAsync(model.AdminId, AdminPermissions.ManageMovies, cancellationToken);
            var movie = await movies.GetByIdAsync(model.Id, cancellationToken);
            if (movie is null)
                return false;

            admin.UpdateMovie(
                movie,
                new Title(model.Title),
                ToDescription(model.Description),
                ToReleaseYear(model.ReleaseYear),
                ToGenre(model.Genre),
                ToPosterUrl(model.PosterUrl));

            return await movies.UpdateAsync(movie, cancellationToken);
        }

        public async Task<bool> DeleteMovieAsync(Guid adminId, Guid movieId, CancellationToken cancellationToken = default)
        {
            var admin = await admins.GetAdminWithPermissionAsync(adminId, AdminPermissions.ManageMovies, cancellationToken);
            var movie = await movies.GetByIdAsync(movieId, cancellationToken);
            if (movie is null)
                return false;

            //если у фильма есть активные оценки или рецензии, домен бросит MovieHasDependenciesException
            admin.DeleteMovie(movie);
            return await movies.UpdateAsync(movie, cancellationToken);
        }


        private static Description? ToDescription(string? value) => value is null ? null : new Description(value);
        private static ReleaseYear? ToReleaseYear(int? value) => value is null ? null : new ReleaseYear(value.Value);
        private static Genre? ToGenre(string? value) => value is null ? null : new Genre(value);
        private static PosterUrl? ToPosterUrl(string? value) => value is null ? null : new PosterUrl(value);
    }
}
