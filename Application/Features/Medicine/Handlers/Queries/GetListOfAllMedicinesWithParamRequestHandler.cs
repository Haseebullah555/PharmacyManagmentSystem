using Application.Contracts.Interfaces.Common;
using Application.Dtos.Common;
using Application.Dtos.Medicine;
using Application.Features.Medicine.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Medicine.Handlers.Queries
{
    public class GetListOfAllMedicinesWithParamRequestHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetListOfAllMedicinesWithParamRequest, PaginatedResult<MedicineDto>>
    {
        public async Task<PaginatedResult<MedicineDto>> Handle(GetListOfAllMedicinesWithParamRequest request, CancellationToken cancellationToken)
        {
             var query = _unitOfWork.Medicines.Query().AsNoTracking();

            // Search
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(s => s.GenericName.ToString().Contains(request.Search));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                if (request.SortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    query = request.SortDirection == "desc"
                        ? query.OrderByDescending(s => s.GenericName)
                        : query.OrderBy(s => s.GenericName);
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
                .Select(e => new MedicineDto
                {
                    Id = e.Id,
                    GenericName = e.GenericName,
                    TradeName = e.TradeName,
                    DosageId = e.DosageId,
                    Dosage = e.Dosage.DosageName,
                    CategoryID = e.CategoryID,
                    Category = e.Category.CategoryName,
                    CompanyID = e.CompanyID,
                    Company = e.Company.CompanyName,
                }).ToListAsync(cancellationToken);

            return new PaginatedResult<MedicineDto>
            {
                Data = Dosages,
                Total = total,
                CurrentPage = request.Page,
                PerPage = request.PerPage
            };
        }
    }
}