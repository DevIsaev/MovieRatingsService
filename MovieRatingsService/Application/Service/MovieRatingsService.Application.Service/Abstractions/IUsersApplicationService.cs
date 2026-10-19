using MovieRatingsService.Application.Models.User;

namespace MovieRatingsService.Application.Service.Abstractions
{
    public interface IUsersApplicationService
    {
        Task<IEnumerable<UserModel>> GetUsersAsync(string? username, CancellationToken cancellationToken);
        Task<UserModel?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<UserModel?> CreateUserAsync(CreateUserModel model, CancellationToken cancellationToken);
    }
}
