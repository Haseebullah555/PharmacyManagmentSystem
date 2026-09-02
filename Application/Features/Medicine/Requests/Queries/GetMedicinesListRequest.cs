using Application.Dtos.Medicine;
using MediatR;

namespace Application.Features.Medicine.Handlers.Queries
{
    public class GetMedicinesListRequest : IRequest<List<MedicineDropDownDto>>
    {
        
    }
}