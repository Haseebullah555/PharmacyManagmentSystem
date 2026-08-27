using Application.Dtos.Company;
using MediatR;

namespace Application.Features.Company.Requests.Commands
{
    public class AddCompanyCommand : IRequest
    {
        public AddCompanyDto AddCompanyDto { get; set; }
    }
}
