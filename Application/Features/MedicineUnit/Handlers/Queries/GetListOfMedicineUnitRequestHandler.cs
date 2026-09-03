using Application.Contracts.Interfaces.Common;
using Application.Dtos.Common;
using Application.Features.MedicineUnit.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MedicineUnit.Handlers.Queries
{
    public class GetMedicineUnitsListRequestHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetMedicineUnitsListRequest, List<DropDownDto>>
    {
        public async Task<List<DropDownDto>> Handle(GetMedicineUnitsListRequest request, CancellationToken cancellationToken)
        {
            var medicineUnits = await _unitOfWork.MedicineUnits.Query().AsNoTracking().Select(x => new DropDownDto
            {
                Id = x.Id,
                Name = x.Medicine.GenericName + "-" + x.Unit.Name,

            }).ToListAsync();
            return medicineUnits;
        }
    }
}