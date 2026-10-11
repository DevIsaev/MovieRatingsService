using MovieRatingsService.Application.Models.Base;

namespace MovieRatingsService.Application.Models.User
{
    public record class CreateUserModel(string Username) : ICreateModel;
}
