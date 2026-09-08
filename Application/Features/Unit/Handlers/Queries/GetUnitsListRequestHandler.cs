using Application.Contracts.Interfaces.Common;
using Application.Dtos.Common;
using Application.Features.Unit.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Unit.Handlers.Queries
{
    public class GetUnitsListRequestHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetUnitsListRequest, List<DropDownDto>>
    {
        public async Task<List<DropDownDto>> Handle(GetUnitsListRequest request, CancellationToken cancellationToken)
        {
            var employees = await _unitOfWork.Units.Query().AsNoTracking().Select(x => new DropDownDto
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
            return employees;
        }
    }
}