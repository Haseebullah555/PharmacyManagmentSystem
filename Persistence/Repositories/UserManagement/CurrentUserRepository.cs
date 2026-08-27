using System.Security.Claims;
using Application.Contracts.UserManagement;
using Microsoft.AspNetCore.Http;

namespace Persistence.Repositories.UserManagement
{
    public class CurrentUserRepository : ICurrentUserRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid GetCurrentLoggedInUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User ID not found in token.");

            return Guid.Parse(userId);
        }
    }
}