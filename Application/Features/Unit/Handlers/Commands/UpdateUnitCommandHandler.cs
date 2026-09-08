using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Unit.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Unit.Handlers.Commands
{
    public class UpdateUnitCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICurrentUserRepository _currentUser) : IRequestHandler<UpdateUnitCommand>
    {
        public async Task Handle(UpdateUnitCommand request, CancellationToken cancellationToken)
        {
            var unit = _mapper.Map<Domain.Models.Unit>(request.UpdateUnitDto);
            unit.UpdatedAt = DateTime.UtcNow;
            unit.UpdateBy = _currentUser.GetCurrentLoggedInUserId();
            _unitOfWork.Units.Update(unit);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}