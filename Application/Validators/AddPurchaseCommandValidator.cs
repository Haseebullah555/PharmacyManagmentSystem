using Application.Features.Purchase.Requests.Commands;
using FluentValidation;

namespace Application.Validators
{
    public class AddPurchaseCommandValidator: AbstractValidator<AddPurchaseCommand>
    {
        public AddPurchaseCommandValidator()
        {
            RuleFor(x => x.AddPurchaseDto.SupplierID)
                .GreaterThan(0);

            RuleFor(x => x.AddPurchaseDto.CurrencyID)
                .GreaterThan(0);

            RuleFor(x => x.AddPurchaseDto.Items)
                .NotEmpty()
                .WithMessage("At least one medicine is required.");

            RuleForEach(x => x.AddPurchaseDto.Items)
                .SetValidator(new AddPurchaseItemValidator());
        }
    }
}