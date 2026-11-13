using AutoMapper;
using Domain.ValueObject.Exceptions;
using Domain.ValueObject;
using MovieRatingsService.Application.Models.Review;
using MovieRatingsService.Application.Service.Abstractions;
using MovieRatingsService.Domain.Domain.Entities;
using MovieRatingsService.Domain.Repositories.Abstractions;

namespace MovieRatingsService.Application.Service
{
    public class ReviewsApplicationService(
        IReviewRepository reviews,
        IMovieRepository movies,
        IUserRepository users,
        IMapper mapper) : IReviewsApplicationService
    {
        public async Task<IEnumerable<ReviewModel>> GetReviewsByMovieAsync(Guid movieId, CancellationToken cancellationToken = default)
            => mapper.Map<IEnumerable<ReviewModel>>(await reviews.GetActiveByMovieAsync(movieId, cancellationToken));

        public async Task<IEnumerable<ReviewModel>> GetReviewsByUserAsync(Guid userId, CancellationToken cancellationToken = default)
            => mapper.Map<IEnumerable<ReviewModel>>(await reviews.GetByUserAsync(userId, cancellationToken));

        public async Task<ReviewModel?> GetUserReviewAsync(Guid userId, Guid movieId, CancellationToken cancellationToken = default)
        {
            var review = await reviews.GetByUserAndMovieAsync(userId, movieId, cancellationToken);
            return review is null ? null : mapper.Map<ReviewModel>(review);
        }

        public async Task<ReviewModel?> AddReviewAsync(CreateReviewModel model, CancellationToken cancellationToken = default)
        {
            var user = await users.GetByIdAsync(model.UserId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(User), model.UserId);
            var movie = await movies.GetByIdAsync(model.MovieId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(Movie), model.MovieId);

            //домен проверяет - фильм не удален и активной рецензии от этого пользователя еще нет
            var review = movie.AddReview(user, new ReviewContent(model.Content));

            var created = await reviews.AddAsync(review, cancellationToken);
            return created is null ? null : mapper.Map<ReviewModel>(created);
        }

        public async Task<bool> UpdateReviewAsync(UpdateReviewModel model, CancellationToken cancellationToken = default)
        {
            var user = await users.GetByIdAsync(model.UserId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(User), model.UserId);
            var movie = await movies.GetByIdAsync(model.MovieId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(Movie), model.MovieId);

            //если активной рецензии нет или скрыли, домен бросит EntityNotFoundException
            var review = movie.UpdateReview(user, new ReviewContent(model.Content));
            return await reviews.UpdateAsync(review, cancellationToken);
        }
    
    }
}
