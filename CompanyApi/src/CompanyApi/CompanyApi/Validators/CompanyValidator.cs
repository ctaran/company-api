using CompanyApi.DTOs;
using FluentValidation;

namespace CompanyApi.Validators;

public class CreateCompanyDtoValidator : AbstractValidator<CreateCompanyDto>
{
    public CreateCompanyDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.StockTicker)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Exchange)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Isin)
            .NotEmpty()
            .Length(12)
            .Matches("^[A-Z]{2}[A-Z0-9]{10}$")
            .WithMessage("ISIN must start with 2 letters followed by 10 alphanumeric characters");

        RuleFor(x => x.Website)
            .MaximumLength(255)
            .When(x => x.Website != null);
    }
}

public class UpdateCompanyDtoValidator : AbstractValidator<UpdateCompanyDto>
{
    public UpdateCompanyDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.StockTicker)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Exchange)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Isin)
            .NotEmpty()
            .Length(12)
            .Matches("^[A-Z]{2}[A-Z0-9]{10}$")
            .WithMessage("ISIN must start with 2 letters followed by 10 alphanumeric characters");

        RuleFor(x => x.Website)
            .MaximumLength(255)
            .When(x => x.Website != null);
    }
} 