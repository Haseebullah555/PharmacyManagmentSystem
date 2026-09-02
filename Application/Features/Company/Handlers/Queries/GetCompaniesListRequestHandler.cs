using Application.Contracts.Interfaces.Common;
using Application.Dtos.Common;
using Application.Features.Company.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Company.Handlers.Queries
{
    public class GetCompaniesListRequestHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetCompaniesListRequest, List<DropDownDto>>
    {
        public async Task<List<DropDownDto>> Handle(GetCompaniesListRequest request, CancellationToken cancellationToken)
        {
            var employees = await _unitOfWork.Companies.Query().AsNoTracking().Select(x => new DropDownDto
            {
                Id = x.Id,
                Name = x.CompanyName
            }).ToListAsync();
            return employees;
        }
    }
}