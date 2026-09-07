using Application.Dtos.Common;
using MediatR;

namespace Application.Features.Customer.Requests.Queries
{
    public class GetCustomersListRequest : IRequest<List<DropDownDto>>
    {
        
    }
}