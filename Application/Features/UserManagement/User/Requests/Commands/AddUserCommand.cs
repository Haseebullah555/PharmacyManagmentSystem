using Application.Dtos.UserManagement;
using Application.Dtos.UserManagement.User;
using MediatR;

namespace Application.Features.UserManagement.User.Requests.Commands
{
    public class AddUserCommand : IRequest
    {
        public AddUserDto User { get; set; }
    }
}