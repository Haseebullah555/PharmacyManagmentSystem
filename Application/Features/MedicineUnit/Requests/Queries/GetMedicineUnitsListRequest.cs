using Application.Dtos.Common;
using MediatR;

namespace Application.Features.MedicineUnit.Requests.Queries
{
    public class GetMedicineUnitsListRequest : IRequest<List<DropDownDto>>
    {
        
    }
}