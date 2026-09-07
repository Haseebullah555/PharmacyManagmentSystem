using Application.Dtos.Customer;
using MediatR;

namespace Application.Features.Customer.Requests.Commands
{
    public class AddCustomerCommand : IRequest
    {
        public AddCustomerDto AddCustomerDto { get; set; }
    }
}