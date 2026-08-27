using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Sale.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Sale.Handlers.Commands
{
    public class UpdateSaleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserRepository currentUser) : IRequestHandler<UpdateSaleCommand>
    {
        public async Task Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = mapper.Map<Domain.Models.Sale>(request.UpdateSaleDto);
            sale.UpdatedAt = DateTime.UtcNow;
            sale.UpdateBy = currentUser.GetCurrentLoggedInUserId();
            unitOfWork.Sales.Update(sale);
            await unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
