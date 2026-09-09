using Application.Dtos.Medicine;
using MediatR;

namespace Application.Features.Medicine.Requests.Queries
{
    public class GetMedicinesListRequest : IRequest<List<MedicineDropDownDto>>
    {
        
    }
}