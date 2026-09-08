using Application.Dtos.Unit;
using MediatR;

namespace Application.Features.Unit.Requests.Commands
{
    public class UpdateUnitCommand : IRequest
    {
        public UpdateUnitDto UpdateUnitDto { get; set; }
    }
}