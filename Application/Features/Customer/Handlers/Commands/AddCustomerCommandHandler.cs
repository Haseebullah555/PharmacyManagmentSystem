using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Customer.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Customer.Handlers.Commands
{
    public class AddCustomerCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICurrentUserRepository _currentUser) : IRequestHandler<AddCustomerCommand>
    {
        public async Task Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            var supplier = _mapper.Map<Domain.Models.Customer>(request.AddCustomerDto);
            supplier.CreatedAt = DateTime.UtcNow;
            supplier.CreatedBy = _currentUser.GetCurrentLoggedInUserId();
            await _unitOfWork.Customers.AddAsync(supplier);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}