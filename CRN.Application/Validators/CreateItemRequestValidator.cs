using CRN.Application.DTOs.Item;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN.Application.Validators
{
    public class CreateItemRequestValidator : AbstractValidator<CreateItemRequest>
    {
        public CreateItemRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("ProductId must be greater than 0.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");
        }
    }
}
