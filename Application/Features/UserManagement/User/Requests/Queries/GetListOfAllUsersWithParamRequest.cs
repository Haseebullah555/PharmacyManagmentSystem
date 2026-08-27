using Application.Dtos.Common;
using Application.Dtos.UserManagement;
using MediatR;

namespace Application.Features.UserManagement.Requests.Queries
{
    public class GetListOfAllUsersWithParamRequest : IRequest<PaginatedResult<UserListDto>>
    {
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 10;
    }
}