using Application.Contracts.Interfaces.Common;
using Application.Dtos.UserManagement.Roles;
using Application.Features.UserManagement.Role.Requests.Queries;
using MediatR;

namespace Application.Features.UserManagement.Role.Commands.Queries
{
    public class GetAllPermissionListRequestHandler : IRequestHandler<GetAllPermissionListRequest, List<PermissionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllPermissionListRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<PermissionDto>> Handle(GetAllPermissionListRequest request, CancellationToken cancellationToken)
        {
            var permissions = await _unitOfWork.Roles.GetAllPermissions();
            return permissions;
        }
    }
}