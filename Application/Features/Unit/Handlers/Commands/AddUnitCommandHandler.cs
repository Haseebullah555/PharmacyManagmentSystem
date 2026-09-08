using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Unit.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Unit.Handlers.Commands
{
    public class AddUnitCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICurrentUserRepository _currentUser) : IRequestHandler<AddUnitCommand>
    {
        public async Task Handle(AddUnitCommand request, CancellationToken cancellationToken)
        {
            var unit = _mapper.Map<Domain.Models.Unit>(request.AddUnitDto);
            unit.CreatedAt = DateTime.UtcNow;
            unit.CreatedBy = _currentUser.GetCurrentLoggedInUserId();
            await _unitOfWork.Units.AddAsync(unit);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}