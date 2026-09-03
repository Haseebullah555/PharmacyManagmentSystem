using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.MedicineUnit.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.MedicineUnit.Handlers.Commands
{
    public class UpdateMedicineUnitCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserRepository currentUser) : IRequestHandler<UpdateMedicineUnitCommand>
    {
        public async Task Handle(UpdateMedicineUnitCommand request, CancellationToken cancellationToken)
        {
            var medicineUnit = mapper.Map<Domain.Models.MedicineUnit>(request.UpdateMedicineUnitDto);
            medicineUnit.UpdatedAt = DateTime.UtcNow;
            medicineUnit.UpdateBy = currentUser.GetCurrentLoggedInUserId();
            unitOfWork.MedicineUnits.Update(medicineUnit);
            await unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
