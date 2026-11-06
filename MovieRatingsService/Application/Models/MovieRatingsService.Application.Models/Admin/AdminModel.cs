using MovieRatingsService.Application.Models.Base;

namespace MovieRatingsService.Application.Models.Admin
{
    public record class AdminModel(Guid Id, string Username, string Permissions) : IModel<Guid>;
}
