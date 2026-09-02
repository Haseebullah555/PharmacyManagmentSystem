using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Supplier.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Supplier.Handlers.Commands
{
    public class AddSupplierCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICurrentUserRepository _currentUser) : IRequestHandler<AddSupplierCommand>
    {
        public async Task Handle(AddSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = _mapper.Map<Domain.Models.Supplier>(request.AddSupplierDto);
            supplier.CreatedAt = DateTime.UtcNow;
            supplier.CreatedBy = _currentUser.GetCurrentLoggedInUserId();
            await _unitOfWork.Suppliers.AddAsync(supplier);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}