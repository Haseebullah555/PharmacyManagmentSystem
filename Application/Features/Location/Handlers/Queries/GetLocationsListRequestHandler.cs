using Application.Contracts.Interfaces.Common;
using Application.Dtos.Common;
using Application.Features.Location.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Location.Handlers.Queries
{
    public class GetLocationsListRequestHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetLocationsListRequest, List<DropDownDto>>
    {
        public async Task<List<DropDownDto>> Handle(GetLocationsListRequest request, CancellationToken cancellationToken)
        {
            var Locations = await _unitOfWork.Locations.Query().AsNoTracking().Select(x => new DropDownDto
            {
                Id = x.Id,
                Name = x.LocationName
            }).ToListAsync();
            return Locations;
        }
    }
}