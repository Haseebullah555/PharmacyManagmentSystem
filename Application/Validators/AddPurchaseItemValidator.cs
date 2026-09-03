using Application.Dtos.PurchaseItem;
using FluentValidation;

namespace Application.Validators
{
   public class AddPurchaseItemValidator : AbstractValidator<AddPurchaseItemDto>
{
    public AddPurchaseItemValidator()
    {
        RuleFor(x => x.MedicineID)
            .GreaterThan(0);

        RuleFor(x => x.MedicineUnitID)
            .GreaterThan(0);

        RuleFor(x => x.Quantity)
            .GreaterThan(0);

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0);

        RuleFor(x => x.BatchNumber)
            .NotEmpty();

        RuleFor(x => x.LocationID)
            .GreaterThan(0);
    }
}
}