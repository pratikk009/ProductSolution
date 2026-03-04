using FluentValidation;
using Application.DTOs;

public class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product Name is required")
            .MaximumLength(100);

        RuleFor(x => x.Price)
            .GreaterThan(0);
    }
}