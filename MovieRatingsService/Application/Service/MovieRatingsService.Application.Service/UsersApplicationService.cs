using AutoMapper;
using Domain.ValueObject;
using MovieRatingsService.Application.Models.User;
using MovieRatingsService.Application.Service.Abstractions;
using MovieRatingsService.Domain.Domain.Entities;
using MovieRatingsService.Domain.Repositories.Abstractions;

namespace MovieRatingsService.Application.Service
{
    public class UsersApplicationService(IUserRepository repository, IMapper mapper) : IUsersApplicationService
    {
        public async Task<IEnumerable<UserModel>> GetUsersAsync(CancellationToken cancellationToken = default)
            => (await repository.GetAllAsync(cancellationToken, asNoTracking: true))
                .Select(mapper.Map<UserModel>);

        public async Task<UserModel?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await repository.GetByIdAsync(id, cancellationToken);
            return user is null ? null : mapper.Map<UserModel>(user);
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync(string? username, CancellationToken cancellationToken = default)
        {
            var users = string.IsNullOrWhiteSpace(username)
                ? await repository.GetAllAsync(cancellationToken, asNoTracking: true)
                : await repository.SearchByUsernameAsync(username.Trim(), cancellationToken);

            return mapper.Map<IEnumerable<UserModel>>(users);
        }

        public async Task<UserModel?> CreateUserAsync(CreateUserModel model, CancellationToken cancellationToken = default)
        {
            // value object проверяет строку (пусто, длина) и бросает исключение
            var username = new Username(model.Username);

            //if (await repository.GetByUsernameAsync(username.Value, cancellationToken) is not null)
            //    throw new ConflictException($"Пользователь '{username.Value}' уже существует.");

            var created = await repository.AddAsync(new User(username), cancellationToken);
            return created is null ? null : mapper.Map<UserModel>(created);
        }
    }
}
