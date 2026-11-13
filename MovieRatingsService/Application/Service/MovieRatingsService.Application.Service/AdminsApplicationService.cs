using AutoMapper;
using Domain.ValueObject;
using MovieRatingsService.Application.Models.Admin;
using MovieRatingsService.Application.Service.Abstractions;
using MovieRatingsService.Application.Service.Exceptions;
using MovieRatingsService.Domain.Domain.Entities;
using MovieRatingsService.Domain.Repositories.Abstractions;

namespace MovieRatingsService.Application.Service
{
    public class AdminsApplicationService(IAdminRepository repository, IMapper mapper) : IAdminsApplicationService
    {
        public async Task<IEnumerable<AdminModel>> GetAdminsAsync(CancellationToken cancellationToken = default)
            => mapper.Map<IEnumerable<AdminModel>>(await repository.GetAllAsync(cancellationToken, asNoTracking: true));

        public async Task<AdminModel?> GetAdminByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var admin = await repository.GetByIdAsync(id, cancellationToken);
            return admin is null ? null : mapper.Map<AdminModel>(admin);
        }

        public async Task<AdminModel?> GetAdminByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            var admin = await repository.GetByUsernameAsync(username, cancellationToken);
            return admin is null ? null : mapper.Map<AdminModel>(admin);
        }

        public async Task<AdminModel?> CreateAdminAsync(CreateAdminModel model, CancellationToken cancellationToken = default)
        {
            //value objects проверяют строки и бросают исключения
            var admin = new Admin(new Username(model.Username), new Permissions(model.Permissions));

            var created = await repository.AddAsync(admin, cancellationToken);
            return created is null ? null : mapper.Map<AdminModel>(created);
        }

        public async Task<bool> UpdateAdminPermissionsAsync(Guid id, string permissions, CancellationToken cancellationToken = default)
        {
            var admin = await repository.GetByIdAsync(id, cancellationToken);
            if (admin is null)
                return false;

            admin.UpdatePermissions(new Permissions(permissions));
            return await repository.UpdateAsync(admin, cancellationToken);
        }

        public async Task<bool> DeleteAdminAsync(Guid id, CancellationToken cancellationToken = default)
            => await repository.DeleteAsync(id, cancellationToken);
    }
}
