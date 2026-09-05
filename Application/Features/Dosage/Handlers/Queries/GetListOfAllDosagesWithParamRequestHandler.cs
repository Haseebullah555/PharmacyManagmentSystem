using Application.Contracts.Interfaces.Common;
using Application.Dtos.Dosage;
using Application.Dtos.Common;
using Application.Features.Dosage.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Dosage.Handlers.Queries
{
    public class GetListOfAllDosagesWithParamRequestHandler : IRequestHandler<GetListOfAllDosagesWithParamRequest, PaginatedResult<DosageDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetListOfAllDosagesWithParamRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PaginatedResult<DosageDto>> Handle(GetListOfAllDosagesWithParamRequest request, CancellationToken cancellationToken)
        {
             var query = _unitOfWork.Dosages.Query().AsNoTracking();

            // Search
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(s => s.DosageName.ToString().Contains(request.Search));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                if (request.SortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    query = request.SortDirection == "desc"
                        ? query.OrderByDescending(s => s.DosageName)
                        : query.OrderBy(s => s.DosageName);
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
            var Dosages = await query
                .Skip((request.Page - 1) * request.PerPage)
                .Take(request.PerPage)
                .Select(e => new DosageDto
                {
                    Id = e.Id,
                    DosageName = e.DosageName,
                    CategoryId = e.CategoryId,
                    Category = e.Category.CategoryName,
                }).ToListAsync(cancellationToken);


            return new PaginatedResult<DosageDto>
            {
                Data = Dosages,
                Total = total,
                CurrentPage = request.Page,
                PerPage = request.PerPage
            };
        }
    }
}