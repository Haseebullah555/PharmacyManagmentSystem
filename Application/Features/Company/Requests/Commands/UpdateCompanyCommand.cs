using Application.Dtos.Company;
using MediatR;

namespace Application.Features.Company.Requests.Commands
{
    public class UpdateCompanyCommand : IRequest
    {
        public UpdateCompanyDto UpdateCompanyDto { get; set; }
    }
}
