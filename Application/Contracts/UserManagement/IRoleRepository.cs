using Application.Contracts.Interfaces.Common;
using Application.Dtos.Common;
using Application.Dtos.UserManagement.Roles;
using Domain.Models.UserManagement;

namespace Application.Contracts.UserManagement
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<PaginatedResult<RolesListDto>> GetAllRolesList(
            int page,
            int perPage,
            string search,
            string? sortBy,
            string? sortDirection
        );
        Task<List<PermissionDto>> GetAllPermissions();
        Task<List<Role>> GetByIdsAsync(
        IEnumerable<Guid> roleIds,
        CancellationToken cancellationToken);
        Task AssignPermissionsToRoleAsync(Guid roleId, List<Guid> permissionIds);
    }
}