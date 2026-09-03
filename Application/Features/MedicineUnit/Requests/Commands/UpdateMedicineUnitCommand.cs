using Application.Dtos.MedicineUnit;
using MediatR;

namespace Application.Features.MedicineUnit.Requests.Commands
{
    public class UpdateMedicineUnitCommand : IRequest
    {
        public UpdateMedicineUnitDto UpdateMedicineUnitDto { get; set; }
    }
}
