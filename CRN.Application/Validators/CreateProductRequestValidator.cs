using CRN.Application.DTOs.Product;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN.Application.Validators
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty()
                .WithMessage("Product name is required.")
                .MaximumLength(255)
                .WithMessage("Product name cannot exceed 255 characters.");
        }
    }
}
