using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Expense.Requests.Commands;
using Application.Features.Response;
using Application.Helper;
using AutoMapper;
using MediatR;

namespace Application.Features.Expense.Handlers.Commands
{
    public class AddExpenseCommandHandler(IUnitOfWork _unitOfWork, ICurrentUserRepository _currentUser, IMapper _mapper) : IRequestHandler<AddExpenseCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(AddExpenseCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var userId = _currentUser.GetCurrentLoggedInUserId();

                // Expense
                var expense = _mapper.Map<Domain.Models.Expense>(request.AddExpenseDto);
                expense.CreatedAt = DateTime.UtcNow;
                expense.CreatedBy = userId;
                expense.Date = PersainDateHelper.ConvertToDateOnly(request.AddExpenseDto.Date);

                await _unitOfWork.Expenses.AddAsync(expense);
                await _unitOfWork.SaveAsync(cancellationToken);

                // Financial Transaction
                // var transactionEntity = new Transaction
                // {
                //     TransactionType = TransactionType.Out,
                //     ReferenceType = ReferenceType.Expences,
                //     ReferenceId = expense.Id,
                //     Amount = expense.Amount,
                //     Date = expense.Date,
                //     CreatedAt = DateTime.UtcNow,
                //     CreatedBy = userId
                // };

                // await _unitOfWork.Transactions.AddAsync(transactionEntity);
                await _unitOfWork.SaveAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return new BaseCommandResponse
                {
                    Message = "added-successfully",
                    Success = true
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