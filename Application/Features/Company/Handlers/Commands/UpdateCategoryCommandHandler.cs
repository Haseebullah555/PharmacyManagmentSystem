using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Company.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Company.Handlers.Commands
{
    public class UpdateCompanyCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICurrentUserRepository _currentUser) : IRequestHandler<UpdateCompanyCommand>
    {
        public async Task Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = _mapper.Map<Domain.Models.Company>(request.UpdateCompanyDto);
            company.UpdatedAt = DateTime.UtcNow;
            company.UpdateBy = _currentUser.GetCurrentLoggedInUserId();
            _unitOfWork.Companies.Update(company);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}