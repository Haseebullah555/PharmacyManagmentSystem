using Application.Dtos.Common;
using MediatR;

namespace Application.Features.Location.Requests.Queries
{
    public class GetLocationsListRequest : IRequest<List<DropDownDto>>
    {
        
    }
}