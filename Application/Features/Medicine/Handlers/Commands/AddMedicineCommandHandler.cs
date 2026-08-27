using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Medicine.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Medicine.Handlers.Commands
{
    public class AddMedicineCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserRepository currentUser) : IRequestHandler<AddMedicineCommand>
    {
        public async Task Handle(AddMedicineCommand request, CancellationToken cancellationToken)
        {
            var medicine = mapper.Map<Domain.Models.Medicine>(request.AddMedicineDto);
            medicine.CreatedAt = DateTime.UtcNow;
            medicine.CreatedBy = currentUser.GetCurrentLoggedInUserId();
            await unitOfWork.Medicines.AddAsync(medicine);
            await unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
