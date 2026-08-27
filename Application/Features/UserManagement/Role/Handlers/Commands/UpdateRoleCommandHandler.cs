using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.UserManagement.Role.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.UserManagement.Role.Handlers.Commands
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserRepository _currentUser;

        public UpdateRoleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserRepository currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }
        public async Task Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = _mapper.Map<Domain.Models.UserManagement.Role>(request.Role);
            role.UpdatedAt = DateTime.UtcNow;
            // role.UpdateBy = _currentUser.GetCurrentLoggedInUserId();
            _unitOfWork.Roles.Update(role);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}