using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Category.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Category.Handlers.Commands
{
    public class AddCategoryCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICurrentUserRepository _currentUser) : IRequestHandler<AddCategoryCommand>
    {
        public  async Task Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Domain.Models.Category>(request.AddCategoryDto);
            category.CreatedAt = DateTime.UtcNow;
            category.CreatedBy = _currentUser.GetCurrentLoggedInUserId();
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}