using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Medicine.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Medicine.Handlers.Commands
{
    public class UpdateMedicineCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserRepository currentUser) : IRequestHandler<UpdateMedicineCommand>
    {
        public async Task Handle(UpdateMedicineCommand request, CancellationToken cancellationToken)
        {
            var medicine = mapper.Map<Domain.Models.Medicine>(request.UpdateMedicineDto);
            medicine.UpdatedAt = DateTime.UtcNow;
            medicine.UpdateBy = currentUser.GetCurrentLoggedInUserId();
            unitOfWork.Medicines.Update(medicine);
            await unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
