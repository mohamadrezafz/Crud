using Crud.Application.Common.Extentions;
using Crud.Application.Common.Interfaces;
using Crud.Application.Customers.Commands.UpdateCustomer;
using Crud.Application.Resources;
using Crud.Domain.Entities;
using FluentValidation;
using IbanNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using PhoneNumbers;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace Crud.Application.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateCustomerCommandValidator(IApplicationDbContext context )
    {
        _context = context;

        RuleFor(command => command.FirstName)
        .NotEmpty()
           .WithMessage(e => string.Format(Messages.IsRequired, nameof(e.FirstName)))
           .MaximumLength(50)
           .WithMessage(e => string.Format(Messages.MaximumLength, nameof(e.FirstName)))
           .MustAsync(IsUniqueCombination)
           .WithMessage(e => Messages.UniqueFirstNameLastNameBirthdate);

        RuleFor(command => command.LastName)
            .NotEmpty()
            .WithMessage(e => string.Format(Messages.IsRequired, nameof(e.LastName)))
            .MaximumLength(50)
            .WithMessage(e => string.Format(Messages.MaximumLength, nameof(e.LastName)));

        RuleFor(command => command.DateOfBirth)
            .NotEmpty()
            .WithMessage(e => string.Format(Messages.IsRequired, nameof(e.DateOfBirth)));

        RuleFor(command => command.BankAccountNumber)
            .NotEmpty()
            .WithMessage(e => string.Format(Messages.IsRequired, nameof(e.BankAccountNumber)))
            .Must(Utilities.IsValidIban)
            .WithMessage(Messages.IBANValidation);

        RuleFor(command => command.PhoneNumber)
            .NotEmpty()
            .WithMessage(e => string.Format(Messages.IsRequired, nameof(e.PhoneNumber)))
            .Must(Utilities.IsPhoneNumberValid)
            .WithMessage(e => Messages.PhoneNumberValidation);

        RuleFor(command => command.Email)
            .NotEmpty()
            .WithMessage(e => string.Format(Messages.IsRequired, nameof(e.Email)))
            .MustAsync(IsUniqueEmail)
            .WithMessage(Messages.EmailValidation)
        .EmailAddress() ;



    }

    private async Task<bool> IsUniqueEmail(string email, CancellationToken cancellationToken) =>
       await _context.Customers.AllAsync(x => x.Email != email, cancellationToken);



    private async Task<bool> IsUniqueCombination(CreateCustomerCommand command , string firstName , CancellationToken cancellationToken)
    {
        var customer = await _context.Customers
            .Where(x => x.FirstName == firstName && x.LastName == command.LastName && x.DateOfBirth == command.DateOfBirth)
            .FirstOrDefaultAsync(cancellationToken);

        return customer != null ? false : true;

    }



}
