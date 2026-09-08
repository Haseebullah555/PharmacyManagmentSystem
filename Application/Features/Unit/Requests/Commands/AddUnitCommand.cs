using Application.Dtos.Unit;
using MediatR;

namespace Application.Features.Unit.Requests.Commands
{
    public class AddUnitCommand : IRequest
    {
        public AddUnitDto AddUnitDto { get; set; }
    }
}