using Application.Contracts.Interfaces.Common;
using Application.Dtos.Common;
using Application.Dtos.UserManagement;
using Application.Features.UserManagement.Requests.Queries;
using MediatR;

namespace Application.Features.UserManagement.Commands.Queries
{
    public class GetListOfAllUsersWithParamRequestHandler : IRequestHandler<GetListOfAllUsersWithParamRequest, PaginatedResult<UserListDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetListOfAllUsersWithParamRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PaginatedResult<UserListDto>> Handle(GetListOfAllUsersWithParamRequest request, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.Users.GetAllUsers(request.Page, request.PerPage, request.Search, request.SortBy, request.SortDirection);
            return users;
        }
    }
}