using Application.Dtos.MedicineUnit;
using MediatR;

namespace Application.Features.MedicineUnit.Requests.Commands
{
    public class AddMedicineUnitCommand : IRequest
    {
        public AddMedicineUnitDto AddMedicineUnitDto { get; set; }
    }
}
