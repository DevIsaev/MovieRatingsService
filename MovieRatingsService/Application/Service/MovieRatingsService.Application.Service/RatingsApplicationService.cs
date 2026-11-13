using AutoMapper;
using Domain.ValueObject.Exceptions;
using Domain.ValueObject;
using MovieRatingsService.Application.Models.Rating;
using MovieRatingsService.Application.Service.Abstractions;
using MovieRatingsService.Domain.Domain.Entities;
using MovieRatingsService.Domain.Repositories.Abstractions;

namespace MovieRatingsService.Application.Service
{
    public class RatingsApplicationService(
        IRatingRepository ratings,
        IMovieRepository movies,
        IUserRepository users,
        IMapper mapper) : IRatingsApplicationService
    {
        public async Task<IEnumerable<RatingModel>> GetRatingsByMovieAsync(Guid movieId, CancellationToken cancellationToken = default)
            => mapper.Map<IEnumerable<RatingModel>>(await ratings.GetActiveByMovieAsync(movieId, cancellationToken));

        public async Task<IEnumerable<RatingModel>> GetRatingsByUserAsync(Guid userId, CancellationToken cancellationToken = default)
            => mapper.Map<IEnumerable<RatingModel>>(await ratings.GetActiveByUserAsync(userId, cancellationToken));

        public async Task<RatingModel?> GetUserRatingAsync(Guid userId, Guid movieId, CancellationToken cancellationToken = default)
        {
            var rating = await ratings.GetByUserAndMovieAsync(userId, movieId, cancellationToken);
            return rating is null ? null : mapper.Map<RatingModel>(rating);
        }

        public async Task<RatingModel?> RateMovieAsync(CreateRatingModel model, CancellationToken cancellationToken = default)
        {
            var user = await users.GetByIdAsync(model.UserId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(User), model.UserId);
            var movie = await movies.GetByIdAsync(model.MovieId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(Movie), model.MovieId);

            //домен проверяет - фильм не удален и активной оценки от этого пользователя еще нет
            var rating = movie.AddRating(user, new Score(model.Score));

            var created = await ratings.AddAsync(rating, cancellationToken);
            return created is null ? null : mapper.Map<RatingModel>(created);
        }

        public async Task<bool> UpdateRatingAsync(UpdateRatingModel model, CancellationToken cancellationToken = default)
        {
            var user = await users.GetByIdAsync(model.UserId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(User), model.UserId);
            var movie = await movies.GetByIdAsync(model.MovieId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(Movie), model.MovieId);

            //если активной оценки нет, домен бросит EntityNotFoundException
            var rating = movie.UpdateRating(user, new Score(model.Score));
            return await ratings.UpdateAsync(rating, cancellationToken);
        }
    }
}
