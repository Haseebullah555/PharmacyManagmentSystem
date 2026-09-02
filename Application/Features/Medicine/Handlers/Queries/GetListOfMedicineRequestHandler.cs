using Application.Contracts.Interfaces.Common;
using Application.Dtos.Medicine;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Medicine.Handlers.Queries
{
    public class GetMedicinesListRequestHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetMedicinesListRequest, List<MedicineDropDownDto>>
    {
        public async Task<List<MedicineDropDownDto>> Handle(GetMedicinesListRequest request, CancellationToken cancellationToken)
        {
            var medicines = await _unitOfWork.Medicines.Query().AsNoTracking().Select(x => new MedicineDropDownDto
            {
                Id = x.Id,
                MedicineName = x.GenericName + "-" + x.Category.CategoryName + "-" + x.Dosage.DosageName,

            }).ToListAsync();
            return medicines;
        }
    }
}