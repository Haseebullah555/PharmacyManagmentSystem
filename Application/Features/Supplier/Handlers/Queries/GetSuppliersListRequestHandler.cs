using Application.Contracts.Interfaces.Common;
using Application.Dtos.Common;
using Application.Features.Supplier.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Supplier.Handlers.Queries
{
    public class GetSuppliersListRequestHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetSuppliersListRequest, List<DropDownDto>>
    {
        public async Task<List<DropDownDto>> Handle(GetSuppliersListRequest request, CancellationToken cancellationToken)
        {
            var employees = await _unitOfWork.Suppliers.Query().AsNoTracking().Select(x => new DropDownDto
            {
                Id = x.Id,
                Name = x.SupplierName
            }).ToListAsync();
            return employees;
        }
    }
}