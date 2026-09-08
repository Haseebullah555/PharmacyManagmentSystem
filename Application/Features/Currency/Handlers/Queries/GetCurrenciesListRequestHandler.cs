using Application.Contracts.Interfaces.Common;
using Application.Dtos.Common;
using Application.Features.Currency.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Currency.Handlers.Queries
{
    public class GetCurrenciesListRequestHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetCurrenciesListRequest, List<DropDownDto>>
    {
        public async Task<List<DropDownDto>> Handle(GetCurrenciesListRequest request, CancellationToken cancellationToken)
        {
            var employees = await _unitOfWork.Currencies.Query().AsNoTracking().Select(x => new DropDownDto
            {
                Id = x.Id,
                Name = x.CurrencyName
            }).ToListAsync();
            return employees;
        }
    }
}