using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Supplier.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Supplier.Handlers.Commands
{
    public class UpdateSupplierCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICurrentUserRepository _currentUser) : IRequestHandler<UpdateSupplierCommand>
    {
        public async Task Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = _mapper.Map<Domain.Models.Supplier>(request.UpdateSupplierDto);
            supplier.UpdatedAt = DateTime.UtcNow;
            supplier.UpdateBy = _currentUser.GetCurrentLoggedInUserId();
            _unitOfWork.Suppliers.Update(supplier);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}