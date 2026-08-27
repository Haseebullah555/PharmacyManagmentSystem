using Application.Dtos.Medicine;
using MediatR;

namespace Application.Features.Medicine.Requests.Commands
{
    public class AddMedicineCommand : IRequest
    {
        public AddMedicineDto AddMedicineDto { get; set; }
    }
}
