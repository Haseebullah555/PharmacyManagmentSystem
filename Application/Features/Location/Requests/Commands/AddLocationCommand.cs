using Application.Dtos.Location;
using MediatR;

namespace Application.Features.Location.Requests.Commands
{
    public class AddLocationCommand : IRequest
    {
        public AddLocationDto AddLocationDto { get; set; }
    }
}