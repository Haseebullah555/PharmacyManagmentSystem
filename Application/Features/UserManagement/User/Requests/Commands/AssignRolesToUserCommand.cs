using Application.Dtos.UserManagement.User;
using MediatR;

namespace Application.Features.UserManagement.User.Requests.Commands
{
    public class AssignRolesToUserCommand : IRequest
    {
        public AssignRolesToUserDto Roles { get; set; }
    }
}