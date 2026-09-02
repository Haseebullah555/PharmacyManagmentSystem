using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Company.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Company.Handlers.Commands
{
    public class AddCompanyCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICurrentUserRepository _currentUser) : IRequestHandler<AddCompanyCommand>
    {
        public  async Task Handle(AddCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = _mapper.Map<Domain.Models.Company>(request.AddCompanyDto);
            company.CreatedAt = DateTime.UtcNow;
            company.CreatedBy = _currentUser.GetCurrentLoggedInUserId();
            await _unitOfWork.Companies.AddAsync(company);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}