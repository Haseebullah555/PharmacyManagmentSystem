using Application.Dtos.UserManagement.Roles;
using MediatR;

namespace Application.Features.UserManagement.Role.Requests.Queries
{
    public class GetAllPermissionListRequest : IRequest<List<PermissionDto>>
    {
        
    }
}