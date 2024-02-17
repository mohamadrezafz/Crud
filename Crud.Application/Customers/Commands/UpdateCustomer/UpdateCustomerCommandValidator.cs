

using Crud.Application.Common.Extentions;
using Crud.Application.Common.Interfaces;
using Crud.Application.Resources;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Crud.Application.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Validator for UpdateCustomerCommand to ensure the validity of updated customer data.
    /// </summary>

    public UpdateCustomerCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        // Validation rules for various properties of the UpdateCustomerCommand
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
            .EmailAddress();
    }

    // Custom validation method to check if the email is unique for the updated customer
    private async Task<bool> IsUniqueEmail(UpdateCustomerCommand command, string email, CancellationToken cancellationToken)
    {

        var dbEmail = await _context.Customers.Where(c => c.Id == command.Id).Select(c => c.Email).FirstOrDefaultAsync(cancellationToken);
        if (string.Equals(dbEmail, email)) return true;
        return await _context.Customers.AllAsync(c => c.Email != email, cancellationToken);
    }

    // Custom validation method to check if the combination of first name, last name, and date of birth is unique for the updated customer
    private async Task<bool> IsUniqueCombination(UpdateCustomerCommand command, string firstName, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers
            .Where(x => x.FirstName == firstName && x.LastName == command.LastName && x.DateOfBirth == command.DateOfBirth)
            .FirstOrDefaultAsync(cancellationToken);

        if (customer == null)
            return true;
        else if (customer.Id == command.Id) 
            return true;
        else return false;
    }


}