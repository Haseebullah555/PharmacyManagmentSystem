using Application.Dtos.UserManagement.Roles;
using MediatR;

namespace Application.Features.UserManagement.Role.Requests.Commands
{
    public class AssignPermissionsToRoleCommand : IRequest
    {
        public AssignPermissionsToRoleDto Permissions { get; set; }
    }
}