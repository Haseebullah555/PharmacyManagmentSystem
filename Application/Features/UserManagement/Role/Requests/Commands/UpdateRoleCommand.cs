using Application.Dtos.UserManagement.Roles;
using MediatR;

namespace Application.Features.UserManagement.Role.Requests.Commands
{
    public class UpdateRoleCommand : IRequest
    {
      public UpdateRoleDto Role { get; set; }  
    }
}