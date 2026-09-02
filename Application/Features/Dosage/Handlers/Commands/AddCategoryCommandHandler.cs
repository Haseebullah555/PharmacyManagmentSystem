using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Dosage.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Dosage.Handlers.Commands
{
    public class AddDosageCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICurrentUserRepository _currentUser) : IRequestHandler<AddDosageCommand>
    {
        public  async Task Handle(AddDosageCommand request, CancellationToken cancellationToken)
        {
            var dosage = _mapper.Map<Domain.Models.Dosage>(request.AddDosageDto);
            dosage.CreatedAt = DateTime.UtcNow;
            dosage.CreatedBy = _currentUser.GetCurrentLoggedInUserId();
            await _unitOfWork.Dosages.AddAsync(dosage);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}