using Application.Dtos.Common;
using MediatR;

namespace Application.Features.Supplier.Requests.Queries
{
    public class GetSuppliersListRequest : IRequest<List<DropDownDto>>
    {
        
    }
}