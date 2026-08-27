using Application.Contracts.Interfaces.Common;
using Application.Dtos.Category;
using Application.Dtos.Common;
using Application.Features.Category.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Category.Handlers.Queries
{
    public class GetListOfAllCategoriesWithParamRequestHandler : IRequestHandler<GetListOfAllCategoriesWithParamRequest, PaginatedResult<CategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetListOfAllCategoriesWithParamRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PaginatedResult<CategoryDto>> Handle(GetListOfAllCategoriesWithParamRequest request, CancellationToken cancellationToken)
        {
             var query = _unitOfWork.Categories.Query().AsNoTracking();

            // Search
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(s => s.CategoryName.ToString().Contains(request.Search));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                if (request.SortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    query = request.SortDirection == "desc"
                        ? query.OrderByDescending(s => s.CategoryName)
                        : query.OrderBy(s => s.CategoryName);
                }
                else if (request.SortBy.Equals("id", StringComparison.OrdinalIgnoreCase))
                {
                    query = request.SortDirection == "desc"
                        ? query.OrderByDescending(s => s.Id)
                        : query.OrderBy(s => s.Id);
                }
            }
            else
            {
                // Default sort (optional)
                query = query.OrderBy(s => s.Id);
            }

            // Total count (before pagination)
            var total = await query.CountAsync(cancellationToken);

            // Pagination
            var Categories = await query
                .Skip((request.Page - 1) * request.PerPage)
                .Take(request.PerPage)
                .Select(e => new CategoryDto
                {
                    Id = e.Id,
                    CategoryName = e.CategoryName,
                }).ToListAsync(cancellationToken);


            return new PaginatedResult<CategoryDto>
            {
                Data = Categories,
                Total = total,
                CurrentPage = request.Page,
                PerPage = request.PerPage
            };
        }
    }
}