using CRN.Application.DTOs.Item;
using FluentValidation;

namespace CRN.Application.Validators
{
    public class UpdateItemRequestValidator : AbstractValidator<UpdateItemRequest>
    {
        public UpdateItemRequestValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");
        }
    }
}
