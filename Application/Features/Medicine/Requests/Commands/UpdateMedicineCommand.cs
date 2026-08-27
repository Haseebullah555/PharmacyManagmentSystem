using Application.Dtos.Medicine;
using MediatR;

namespace Application.Features.Medicine.Requests.Commands
{
    public class UpdateMedicineCommand : IRequest
    {
        public UpdateMedicineDto UpdateMedicineDto { get; set; }
    }
}
