using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Company.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Company.Handlers.Commands
{
    public class UpdateCompanyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserRepository currentUser) : IRequestHandler<UpdateCompanyCommand>
    {
        public async Task Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = mapper.Map<Domain.Models.Company>(request.UpdateCompanyDto);
            company.UpdatedAt = DateTime.UtcNow;
            company.UpdateBy = currentUser.GetCurrentLoggedInUserId();
            unitOfWork.Companies.Update(company);
            await unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
