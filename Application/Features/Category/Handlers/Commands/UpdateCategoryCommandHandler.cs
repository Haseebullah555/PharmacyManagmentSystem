using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Category.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Category.Handlers.Commands
{
    public class UpdateCategoryCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICurrentUserRepository _currentUser) : IRequestHandler<UpdateCategoryCommand>
    {
        public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Domain.Models.Category>(request.UpdateCategoryDto);
            category.UpdatedAt = DateTime.UtcNow;
            category.UpdateBy = _currentUser.GetCurrentLoggedInUserId();
            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}