using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.UserManagement.User.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.UserManagement.User.Handlers.Commands
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
         private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserRepository _currentUser;
        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserRepository currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }
        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<Domain.Models.UserManagement.User>(request.User);
            user.UpdatedAt = DateTime.UtcNow;
            // user.UpdateBy = _currentUser.GetCurrentLoggedInUserId();
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}