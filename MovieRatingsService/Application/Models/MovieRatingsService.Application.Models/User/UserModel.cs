using MovieRatingsService.Application.Models.Base;
namespace MovieRatingsService.Application.Models.User
{
    public record class UserModel(Guid Id, string Username) : IModel<Guid>;
}
