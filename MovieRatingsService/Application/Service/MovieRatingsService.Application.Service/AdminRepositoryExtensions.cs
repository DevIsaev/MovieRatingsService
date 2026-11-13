using Domain.ValueObject.Exceptions;
using MovieRatingsService.Domain.Domain.Entities;
using MovieRatingsService.Domain.Repositories.Abstractions;

namespace MovieRatingsService.Application.Service
{
    internal static class AdminRepositoryExtensions
    {

        //загрузка администратора и проверка что у него есть нужное право
        //нет администратора - EntityNotFoundException (404), нет права - UnauthorizedOperationException (403)
        public static async Task<Admin> GetAdminWithPermissionAsync(this IAdminRepository admins, Guid adminId, string permission, CancellationToken cancellationToken)
        {
            var admin = await admins.GetByIdAsync(adminId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(Admin), adminId);

            if (!admin.HasPermission(permission))
                throw new UnauthorizedOperationException(
                    $"У администратора {admin.Username.Value} нет права '{permission}'.");

            return admin;
        }
    }
}
