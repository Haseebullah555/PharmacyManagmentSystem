using System.Security.Principal;
using Application.Dtos.UserManagement.Roles;
using MediatR;

namespace Application.Features.UserManagement.Role.Requests.Commands
{
    public class AddRoleCommand : IRequest
    {
        public AddRoleDto Roles { get; set; }
    }
}