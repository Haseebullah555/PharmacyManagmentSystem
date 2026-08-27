using Application.Dtos.Common;
using Application.Dtos.Medicine;
using MediatR;

namespace Application.Features.Medicine.Requests.Queries
{
    public class GetListOfAllMedicinesWithParamRequest : IRequest<PaginatedResult<MedicineDto>>
    {
        public string Search { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 10;
    }
}
