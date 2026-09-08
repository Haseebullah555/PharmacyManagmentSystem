using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Currency.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Currency.Handlers.Commands
{
    public class AddCurrencyCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICurrentUserRepository _currentUser) : IRequestHandler<AddCurrencyCommand>
    {
        public async Task Handle(AddCurrencyCommand request, CancellationToken cancellationToken)
        {
            var currency = _mapper.Map<Domain.Models.Currency>(request.AddCurrencyDto);
            currency.CreatedAt = DateTime.UtcNow;
            currency.CreatedBy = _currentUser.GetCurrentLoggedInUserId();
            await _unitOfWork.Currencies.AddAsync(currency);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}