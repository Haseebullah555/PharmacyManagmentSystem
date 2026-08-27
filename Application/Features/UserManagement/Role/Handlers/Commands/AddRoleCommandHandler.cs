using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.UserManagement.Role.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.UserManagement.Role.Handlers.Commands
{
    public class AddRoleCommandHandler : IRequestHandler<AddRoleCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserRepository _currentUser;

        public AddRoleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserRepository currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }
        public async Task Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var role = _mapper.Map<Domain.Models.UserManagement.Role>(request.Roles);
            role.CreatedAt = DateTime.UtcNow;
            // role.CreatedBy = _currentUser.GetCurrentLoggedInUserId();
            await _unitOfWork.Roles.AddAsync(role);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}