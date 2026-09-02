using Application.Dtos.Supplier;
using MediatR;

namespace Application.Features.Supplier.Requests.Commands
{
    public class AddSupplierCommand : IRequest
    {
        public AddSupplierDto AddSupplierDto { get; set; }
    }
}