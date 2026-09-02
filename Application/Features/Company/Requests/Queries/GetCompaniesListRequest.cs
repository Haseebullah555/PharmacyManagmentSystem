using Application.Dtos.Common;
using MediatR;

namespace Application.Features.Company.Requests.Queries
{
    public class GetCompaniesListRequest : IRequest<List<DropDownDto>>
    {
        
    }
}