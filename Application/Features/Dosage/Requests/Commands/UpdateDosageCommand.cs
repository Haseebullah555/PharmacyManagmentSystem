using Application.Dtos.Dosage;
using MediatR;

namespace Application.Features.Dosage.Requests.Commands
{
    public class UpdateDosageCommand : IRequest
    {
        public UpdateDosageDto UpdateDosageDto { get; set; }
    }
}