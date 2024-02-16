using Crud.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using PhoneNumbers;

namespace Crud.Application.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateCustomerCommandValidator(IApplicationDbContext context , IStringLocalizer<CreateCustomerCommand> localizer)
    {
        _context = context;

        RuleFor(command => command.FirstName)
        .NotEmpty()
           .WithMessage(e => string.Format(localizer["IsRequired"], nameof(e.FirstName)))
           .MaximumLength(50)
           .WithMessage(e => string.Format(localizer["MaximumLength"], nameof(e.FirstName)));

        RuleFor(command => command.LastName)
            .NotEmpty()
            .WithMessage(e => string.Format(localizer["IsRequired"], nameof(e.LastName)))
            .MaximumLength(50)
            .WithMessage(e => string.Format(localizer["MaximumLength"], nameof(e.LastName)));

        RuleFor(command => command.DateOfBirth)
            .NotEmpty()
            .WithMessage(e => string.Format(localizer["IsRequired"], nameof(e.DateOfBirth)));

        RuleFor(command => command.BankAccountNumber)
            .NotEmpty()
            .WithMessage(e => string.Format(localizer["IsRequired"], nameof(e.BankAccountNumber)));

        RuleFor(command => command.PhoneNumber)
            .NotEmpty()
            .WithMessage(e => string.Format(localizer["IsRequired"], nameof(e.PhoneNumber)))
            .Must(IsPhoneNumberValid)
            .WithMessage(e => string.Format(localizer["PhoneNumberValidation"]));

        RuleFor(command => command.Email)
            .NotEmpty()
            .WithMessage(e => string.Format(localizer["IsRequired"], nameof(e.Email)))
            .MustAsync(IsUniqueEmail)
            .WithMessage(e => string.Format(localizer["EmailValidation"]))
            .EmailAddress();
    }

    private bool IsPhoneNumberValid(string phoneNumber)
    {
        // Use Google's LibPhoneNumber to validate mobile numbers
        var phoneUtil = PhoneNumberUtil.GetInstance();
        try
        {
            var parsedNumber = phoneUtil.Parse(phoneNumber, "");
            return phoneUtil.IsValidNumber(parsedNumber);
        }
        catch (Exception)
        {

            return false;
        }
        
    }
    private Task<bool> IsUniqueEmail(string email, CancellationToken cancellationToken) =>
        _context.Customers.AnyAsync(x => x.Email == email, cancellationToken);

}
