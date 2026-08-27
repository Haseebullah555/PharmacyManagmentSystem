using Application.Contracts.Interfaces.Common;
using Application.Features.UserManagement.Role.Requests.Commands;
using MediatR;

namespace Application.Features.UserManagement.Role.Handlers.Commands
{
    public class AssignPermissionsToRoleCommandHandler : IRequestHandler<AssignPermissionsToRoleCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssignPermissionsToRoleCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }
        public Task Handle(AssignPermissionsToRoleCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}