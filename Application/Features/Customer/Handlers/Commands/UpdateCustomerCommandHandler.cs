using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Customer.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Customer.Handlers.Commands
{
    public class UpdateCustomerCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICurrentUserRepository _currentUser) : IRequestHandler<UpdateCustomerCommand>
    {
        public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var supplier = _mapper.Map<Domain.Models.Customer>(request.UpdateCustomerDto);
            supplier.UpdatedAt = DateTime.UtcNow;
            supplier.UpdateBy = _currentUser.GetCurrentLoggedInUserId();
            _unitOfWork.Customers.Update(supplier);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}