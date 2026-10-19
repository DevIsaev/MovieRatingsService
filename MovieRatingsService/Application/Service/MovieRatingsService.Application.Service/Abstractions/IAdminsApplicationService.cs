using MovieRatingsService.Application.Models.Admin;

namespace MovieRatingsService.Application.Service.Abstractions
{
    public interface IAdminsApplicationService
    {
        Task<IEnumerable<AdminModel>> GetAdminsAsync(CancellationToken cancellationToken);
        Task<AdminModel?> GetAdminByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<AdminModel?> GetAdminByUsernameAsync(string username, CancellationToken cancellationToken);
        Task<AdminModel?> CreateAdminAsync(CreateAdminModel model, CancellationToken cancellationToken);
        Task<bool> UpdateAdminPermissionsAsync(Guid id, string permissions, CancellationToken cancellationToken);
        Task<bool> DeleteAdminAsync(Guid id, CancellationToken cancellationToken);
    }
}
