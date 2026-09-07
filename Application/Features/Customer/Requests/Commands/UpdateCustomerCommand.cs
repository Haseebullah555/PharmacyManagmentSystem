using Application.Dtos.Customer;
using MediatR;

namespace Application.Features.Customer.Requests.Commands
{
    public class UpdateCustomerCommand : IRequest
    {
        public UpdateCustomerDto UpdateCustomerDto { get; set; }
    }
}