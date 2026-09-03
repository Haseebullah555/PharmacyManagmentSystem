using Application.Dtos.Common;
using Application.Dtos.MedicineUnit;
using MediatR;

namespace Application.Features.MedicineUnit.Requests.Queries
{
    public class GetListOfAllMedicineUnitsWithParamRequest : IRequest<PaginatedResult<MedicineUnitDto>>
    {
        public string Search { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 10;
    }
}
