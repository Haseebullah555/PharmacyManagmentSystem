using Application.Dtos.Common;
using MediatR;

namespace Application.Features.Category.Requests.Queries
{
    public class GetCategoriesListRequest : IRequest<List<DropDownDto>>
    {
        
    }
}