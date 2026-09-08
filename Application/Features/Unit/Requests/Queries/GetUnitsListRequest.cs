using Application.Dtos.Common;
using MediatR;

namespace Application.Features.Unit.Requests.Queries
{
    public class GetUnitsListRequest : IRequest<List<DropDownDto>>
    {
        
    }
}