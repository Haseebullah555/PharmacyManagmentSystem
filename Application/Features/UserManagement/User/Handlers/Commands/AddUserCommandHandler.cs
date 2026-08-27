using Application.Contracts.Interfaces.Common;
using Application.Features.UserManagement.User.Requests.Commands;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.UserManagement.User.Handlers.Commands
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Domain.Models.UserManagement.User> _passwordHasher;

        public AddUserCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IPasswordHasher<Domain.Models.UserManagement.User> passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            // 1. Validation
            if (await _unitOfWork.Users.ExistsAsync(
                request.User.UserName,
                request.User.Email))
            {
                throw new Exception("User already exists.");
            }

            // 2. Mapping
            var user = _mapper.Map<Domain.Models.UserManagement.User>(request.User);
            user.Id = Guid.NewGuid();
            user.CreatedAt = DateTime.UtcNow;

            // 3. Password hashing
            user.PasswordHash = _passwordHasher.HashPassword(user, request.User.Password);

            // 4. Load roles (✅ FIXED)
            var roles = await _unitOfWork.Roles
                .GetByIdsAsync(request.User.roles, cancellationToken);
            List<Guid> roleIds = roles.Select(r => r.Id).ToList();

            // 5. Assign roles
            await _unitOfWork.Users.AssignRolesToUserAsync(user.Id, roleIds);

            // 6. Persist
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveAsync(cancellationToken);
        }


    }
}