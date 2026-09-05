using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Location.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Location.Handlers.Commands
{
    public class UpdateLocationCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICurrentUserRepository _currentUser) : IRequestHandler<UpdateLocationCommand>
    {
        public async Task Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
        {
            var location = _mapper.Map<Domain.Models.Location>(request.UpdateLocationDto);
            location.UpdatedAt = DateTime.UtcNow;
            location.UpdateBy = _currentUser.GetCurrentLoggedInUserId();
            _unitOfWork.Locations.Update(location);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}