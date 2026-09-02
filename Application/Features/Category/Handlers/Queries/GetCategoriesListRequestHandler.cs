using Application.Contracts.Interfaces.Common;
using Application.Dtos.Common;
using Application.Features.Category.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Category.Handlers.Queries
{
    public class GetCategoriesListRequestHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetCategoriesListRequest, List<DropDownDto>>
    {
        public async Task<List<DropDownDto>> Handle(GetCategoriesListRequest request, CancellationToken cancellationToken)
        {
            var employees = await _unitOfWork.Categories.Query().AsNoTracking().Select(x => new DropDownDto
            {
                Id = x.Id,
                Name = x.CategoryName
            }).ToListAsync();
            return employees;
        }
    }
}