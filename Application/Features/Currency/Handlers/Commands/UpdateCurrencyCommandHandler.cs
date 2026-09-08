using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Currency.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Currency.Handlers.Commands
{
    public class UpdateCurrencyCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICurrentUserRepository _currentUser) : IRequestHandler<UpdateCurrencyCommand>
    {
        public async Task Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
        {
            var currency = _mapper.Map<Domain.Models.Currency>(request.UpdateCurrencyDto);
            currency.UpdatedAt = DateTime.UtcNow;
            currency.UpdateBy = _currentUser.GetCurrentLoggedInUserId();
            _unitOfWork.Currencies.Update(currency);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}