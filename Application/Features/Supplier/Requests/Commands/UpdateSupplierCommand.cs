using Application.Dtos.Supplier;
using MediatR;

namespace Application.Features.Supplier.Requests.Commands
{
    public class UpdateSupplierCommand : IRequest
    {
        public UpdateSupplierDto UpdateSupplierDto { get; set; }
    }
}