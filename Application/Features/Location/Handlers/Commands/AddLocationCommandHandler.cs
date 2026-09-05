using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Location.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Location.Handlers.Commands
{
    public class AddLocationCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICurrentUserRepository _currentUser) : IRequestHandler<AddLocationCommand>
    {
        public  async Task Handle(AddLocationCommand request, CancellationToken cancellationToken)
        {
            var location = _mapper.Map<Domain.Models.Location>(request.AddLocationDto);
            location.CreatedAt = DateTime.UtcNow;
            location.CreatedBy = _currentUser.GetCurrentLoggedInUserId();
            await _unitOfWork.Locations.AddAsync(location);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}