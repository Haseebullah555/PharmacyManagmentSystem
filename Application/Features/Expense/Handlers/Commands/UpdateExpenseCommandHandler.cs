using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Expense.Requests.Commands;
using Application.Features.Response;
using Application.Helper;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Expense.Handlers.Commands
{
    public class UpdateExpenseCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICurrentUserRepository _currentUserRepository) : IRequestHandler<UpdateExpenseCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var expense = await _unitOfWork.Expenses.GetByIdAsync(request.UpdateExpenseDto.Id);

                if (expense == null)
                {
                    return new BaseCommandResponse
                    {
                        Success = false,
                        Message = "not-found"
                    };
                }

                var userId = _currentUserRepository.GetCurrentLoggedInUserId();

                // Update Expense
                _mapper.Map(request.UpdateExpenseDto, expense);
                expense.UpdatedAt = DateTime.UtcNow;
                expense.UpdateBy = userId;

                _unitOfWork.Expenses.Update(expense);

                // Update Financial Transaction
                // var transactionEntity = await _unitOfWork.Transactions
                //     .Query()
                //     .FirstOrDefaultAsync(x =>
                //         x.ReferenceType == ReferenceType.Expences &&
                //         x.ReferenceId == expense.Id,
                //         cancellationToken);

                // if (transactionEntity != null)
                // {
                //     transactionEntity.Amount = expense.Amount;
                //     transactionEntity.Date = expense.Date;
                //     transactionEntity.TransactionType = TransactionType.Out;
                //     transactionEntity.UpdatedAt = DateTime.UtcNow;
                //     transactionEntity.UpdateBy = userId;

                //     _unitOfWork.Transactions.Update(transactionEntity);
                // }

                // await _unitOfWork.SaveAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return new BaseCommandResponse
                {
                    Success = true,
                    Message = "updated-successfully"
                };
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}