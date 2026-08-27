using Application.Dtos.Common;
using Application.Dtos.UserManagement.Roles;
using MediatR;

namespace Application.Features.UserManagement.Role.Requests.Queries
{
    public class GetListOfAllRolesWithParamRequest : IRequest<PaginatedResult<RolesListDto>>
    {
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 10;
    }
}