using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.MedicineUnit.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.MedicineUnit.Handlers.Commands
{
    public class AddMedicineUnitCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserRepository currentUser) : IRequestHandler<AddMedicineUnitCommand>
    {
        public async Task Handle(AddMedicineUnitCommand request, CancellationToken cancellationToken)
        {
            var medicineUnit = mapper.Map<Domain.Models.MedicineUnit>(request.AddMedicineUnitDto);
            medicineUnit.CreatedAt = DateTime.UtcNow;
            medicineUnit.CreatedBy = currentUser.GetCurrentLoggedInUserId();
            await unitOfWork.MedicineUnits.AddAsync(medicineUnit);
            await unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
