using System.Globalization;
using Application.Contracts.Interfaces.Common;
using Application.Dtos.Common;
using Application.Dtos.UserManagement;
using Domain.Models.UserManagement;

namespace Application.Contracts.UserManagement
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<UserListDto> GetUserByUsernameAsync(string username);
        Task<PaginatedResult<UserListDto>> GetAllUsers(
           int page,
            int perPage,
            string search,
            string? sortBy,
            string? sortDirection
        );
        Task AssignRolesToUserAsync(Guid userId, List<Guid> roleIds);
        Task<bool> ExistsAsync(string username, string email);
    }
}