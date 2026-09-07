using Application.Contracts.Interfaces.Common;
using Application.Dtos.Common;
using Application.Features.Customer.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Customer.Handlers.Queries
{
    public class GetCustomersListRequestHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetCustomersListRequest, List<DropDownDto>>
    {
        public async Task<List<DropDownDto>> Handle(GetCustomersListRequest request, CancellationToken cancellationToken)
        {
            var employees = await _unitOfWork.Customers.Query().AsNoTracking().Select(x => new DropDownDto
            {
                Id = x.Id,
                Name = x.CustomerName
            }).ToListAsync();
            return employees;
        }
    }
}