using Application.Contracts.Interfaces.Common;
using Application.Dtos.Common;
using Application.Dtos.UserManagement.Roles;
using Application.Features.UserManagement.Role.Requests.Queries;
using MediatR;

namespace Application.Features.UserManagement.Role.Commands.Queries
{
    public class GetListOfAllRolesWithParamRequestHandler : IRequestHandler<GetListOfAllRolesWithParamRequest, PaginatedResult<RolesListDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetListOfAllRolesWithParamRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PaginatedResult<RolesListDto>> Handle(GetListOfAllRolesWithParamRequest request, CancellationToken cancellationToken)
        {
            var roles = await _unitOfWork.Roles.GetAllRolesList(request.Page, request.PerPage, request.Search, request.SortBy, request.SortDirection);
            return roles;
        }
    }
}