using FluentValidation;
using POS.Application.Dtos.Request;

namespace POS.Application.Validators.Category
{
    public class CategoryValidator : AbstractValidator<CategoryRequestDto>
    {
        public CategoryValidator()
        {
            RuleFor(category => category.Name)
                .NotNull().WithMessage("field cannot be null")
                .NotEmpty().WithMessage("field cannot be empty");

            RuleFor(category => category.Description)
                .NotNull().WithMessage("field cannot be null")
                .NotEmpty().WithMessage("field cannot be empty");
        }
    }
}
