using Application.Contracts.Interfaces.Common;
using Application.Dtos.Common;
using Application.Dtos.MedicineUnit;
using Application.Features.MedicineUnit.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MedicineUnit.Handlers.Queries
{
    public class GetListOfAllMedicineUnitsWithParamRequestHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetListOfAllMedicineUnitsWithParamRequest, PaginatedResult<MedicineUnitDto>>
    {
        public async Task<PaginatedResult<MedicineUnitDto>> Handle(GetListOfAllMedicineUnitsWithParamRequest request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.MedicineUnits.Query().AsNoTracking();

            // Search
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(s => s.Medicine.GenericName.Contains(request.Search));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                if (request.SortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    query = request.SortDirection == "desc"
                        ? query.OrderByDescending(s => s.Medicine.GenericName)
                        : query.OrderBy(s => s.Medicine.GenericName);
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
            var medicineUnits = await query
                .Skip((request.Page - 1) * request.PerPage)
                .Take(request.PerPage)
                .Select(e => new MedicineUnitDto
                {
                    Id = e.Id,
                    Medicine = e.Medicine.GenericName,
                    MedicineID = e.MedicineID,
                    UnitID = e.UnitID,
                    Unit = e.Unit.Name,
                    ConversionFactor = e.ConversionFactor,
                    IsBaseUnit = e.IsBaseUnit,
                    CanPurchase = e.CanPurchase,
                    CanSell = e.CanSell
                }).ToListAsync(cancellationToken);

            return new PaginatedResult<MedicineUnitDto>
            {
                Data = medicineUnits,
                Total = total,
                CurrentPage = request.Page,
                PerPage = request.PerPage
            };
        }
    }
}