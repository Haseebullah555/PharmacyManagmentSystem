using Application.Dtos.Category;
using Application.Dtos.Common;
using MediatR;

namespace Application.Features.Category.Requests.Queries
{
    public class GetListOfAllCategoriesWithParamRequest : IRequest<PaginatedResult<CategoryDto>>
    {
        public string Search { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 10;
    }
}