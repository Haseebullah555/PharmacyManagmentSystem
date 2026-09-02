using Application.Dtos.Common;
using MediatR;

namespace Application.Features.Dosage.Requests.Queries
{
    public class GetDosagesListRequest : IRequest<List<DropDownDto>>
    {
        
    }
}