using Application.Contracts.Interfaces.Common;
using Application.Dtos.Common;
using Application.Features.Dosage.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Dosage.Handlers.Queries
{
    public class GetDosagesListRequestHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetDosagesListRequest, List<DropDownDto>>
    {
        public async Task<List<DropDownDto>> Handle(GetDosagesListRequest request, CancellationToken cancellationToken)
        {
            var dosages = await _unitOfWork.Dosages.Query().AsNoTracking().Select(x => new DropDownDto
            {
                Id = x.Id,
                Name = x.DosageName
            }).ToListAsync();
            return dosages;
        }
    }
}