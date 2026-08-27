using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Company.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Company.Handlers.Commands
{
    public class AddCompanyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserRepository currentUser) : IRequestHandler<AddCompanyCommand>
    {
        public async Task Handle(AddCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = mapper.Map<Domain.Models.Company>(request.AddCompanyDto);
            company.CreatedAt = DateTime.UtcNow;
            company.CreatedBy = currentUser.GetCurrentLoggedInUserId();
            await unitOfWork.Companies.AddAsync(company);
            await unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
