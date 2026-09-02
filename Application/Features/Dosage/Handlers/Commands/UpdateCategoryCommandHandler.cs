using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Dosage.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Dosage.Handlers.Commands
{
    public class UpdateDosageCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICurrentUserRepository _currentUser) : IRequestHandler<UpdateDosageCommand>
    {
        public async Task Handle(UpdateDosageCommand request, CancellationToken cancellationToken)
        {
            var dosage = _mapper.Map<Domain.Models.Dosage>(request.UpdateDosageDto);
            dosage.UpdatedAt = DateTime.UtcNow;
            dosage.UpdateBy = _currentUser.GetCurrentLoggedInUserId();
            _unitOfWork.Dosages.Update(dosage);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}