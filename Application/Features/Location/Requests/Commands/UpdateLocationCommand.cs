using Application.Dtos.Location;
using MediatR;

namespace Application.Features.Location.Requests.Commands
{
    public class UpdateLocationCommand : IRequest
    {
        public UpdateLocationDto UpdateLocationDto { get; set; }
    }
}