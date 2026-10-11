using MovieRatingsService.Application.Models.Base;

namespace MovieRatingsService.Application.Models.Admin
{
    public record class CreateAdminModel(string Username, string Permissions) : ICreateModel;
}
