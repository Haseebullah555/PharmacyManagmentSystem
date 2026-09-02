using Application.Dtos.Dosage;
using MediatR;

namespace Application.Features.Dosage.Requests.Commands
{
    public class AddDosageCommand : IRequest
    {
        public AddDosageDto AddDosageDto { get; set; }
    }
}