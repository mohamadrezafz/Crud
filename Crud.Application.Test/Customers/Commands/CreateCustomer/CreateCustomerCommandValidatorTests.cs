﻿

using Crud.Application.Common.Interfaces;
using Crud.Application.Customers.Commands.CreateCustomer;
using Crud.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using PhoneNumbers;

namespace Crud.Application.Tests.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandValidatorTests
{

    #region privates

    private readonly ICollection<Customer> _customers = new List<Customer>
        {
            new()
            {
                Id = 1,
                FirstName = "Reza",
                LastName = "Tehran",
                DateOfBirth = DateTime.Parse("9/3/1990"),
                PhoneNumber = "+989120259388",
                Email = "reza@gmail.com",
                BankAccountNumber = "LT411593463320934097"
            },
            new()
            {
                Id = 2,
                FirstName = "Mandana",
                LastName = "Shayan",
                DateOfBirth = DateTime.Parse("2/7/2000"),
                PhoneNumber = "+639963187652",
                Email = "mandana@kukala.com",
                BankAccountNumber = "TR3305582GTASOWMCZ0UMYHIU2"
            },
            new()
            {
                Id = 3,
                FirstName = "Nafiseh",
                LastName = "Roshan",
                DateOfBirth = DateTime.Parse("8/10/1990"),
                PhoneNumber = "+622644398525",
                Email = "nafiseh@virginia.edu",
                BankAccountNumber = "GE84LW8259319842791322"
            }
        };

    private readonly IApplicationDbContext _context;

    private readonly CreateCustomerCommandValidator _validator;

    #endregion

    #region ctor
    public CreateCustomerCommandValidatorTests()
    {
        var mockDbSet = _customers.AsQueryable().BuildMockDbSet();
        var mockDbContext = new Mock<IApplicationDbContext>();
        mockDbContext.Setup(x => x.Customers).Returns(mockDbSet.Object);
        _context = mockDbContext.Object;

        _validator = new CreateCustomerCommandValidator(_context);
    }

    #endregion


    #region FirstName and LastName and Birthdate Validation
    [Fact]
    public async Task Default_String_FirstName_Should_Fail()
    {
        var command = new CreateCustomerCommand("", "Firooz", DateTime.Parse("3/24/1994"), "+989120259301", "mohamadreza.firooz@gmail.com", "BA177369567652933471");
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[] { "FirstName is required." });
    }
    [Fact]
    public async Task Max_String_FirstName_Should_Fail()
    {
        var command = new CreateCustomerCommand("sdsdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasdasfdsfsfdsfsfsdfdasd", "Firooz", DateTime.Parse("3/24/1994"), "+989120259301", "mohamadreza.firooz@gmail.com", "BA177369567652933471");
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[] { "FirstName must be less than 50 characters." });
    }

    [Fact]
    public async Task Default_String_LastName_Should_Fail()
    {
        var command = new CreateCustomerCommand("Reza", "", DateTime.Parse("3/24/1994"), "+989120259301", "mohamadreza.firooz@gmail.com", "BA177369567652933471");
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[] { "LastName is required." });
    }

    [Fact]
    public async Task Max_String_LastName_Should_Fail()
    {
        var command = new CreateCustomerCommand("Reza", "sdsdasdasdasdasdasdasdasdasdasdasdasdasdafssfdfdsfsdfsdasdasdasdasdasd", DateTime.Parse("3/24/1994"), "+989120259301", "mohamadreza.firooz@gmail.com", "BA177369567652933471");
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[] { "LastName must be less than 50 characters." });
    }

    [Fact]
    public async Task Used_FirstName_LastName_BirthDate_Should_Fail()
    {
        var command = new CreateCustomerCommand("Reza", "Tehran", DateTime.Parse("9/3/1990"), "+989120259388", "reza2@gmail.com", "LT411593463320934097");
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[] { "User already exists." });
    }

    #endregion

    #region PhoneNumber Validation

    [Fact]
    public async Task Default_String_PhoneNumber_Should_Fail()
    {
        var command = new CreateCustomerCommand("Reza", "Firooz", DateTime.Parse("3/24/1994"), "", "mohamadreza.firooz@gmail.com", "BA177369567652933471");
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]{ "PhoneNumber is required." , "Invalid phone number. Please ensure correct mobile format. e.g. +989120925888" });
    }

    [Theory]
    [InlineData("Test")]
    public async Task Random_PhoneNumber_Should_Fail(string phoneNumber)
    {
        var command = new CreateCustomerCommand("Reza", "Firooz", DateTime.Parse("3/24/1994"), phoneNumber, "mohamadreza.firooz@gmail.com", "BA177369567652933471");
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]{"Invalid phone number. Please ensure correct mobile format. e.g. +989120925888"});
    }

    [Theory]
    [InlineData("+989120259301")]
    public async Task Valid_PhoneNumber_Should_Pass(string phoneNumber)
    {
        var command = new CreateCustomerCommand("Reza", "Firooz", DateTime.Parse("3/24/1994"), phoneNumber, "mohamadreza.firooz@gmail.com", "BA177369567652933471");
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.IsValid.Should().BeTrue();
    }

    #endregion


    #region Email Validation
    [Fact]
    public async Task Default_String_Email_Should_Fail()
    {
        var command = new CreateCustomerCommand("Reza", "Firooz", DateTime.Parse("3/24/1994"), "+989120259645", "", "BA177369567652933471");
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[] {"Email is required.", "'Email' is not a valid email address."});
    }

    [Theory]
    [InlineData("Test")]
    public async Task Random_Email_Should_Fail(string email)
    {
        var command = new CreateCustomerCommand("Reza", "Firooz", DateTime.Parse("3/24/1994"), "+989120259645", email, "BA177369567652933471");
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]{ "'Email' is not a valid email address."});
    }


    [Theory]
    [InlineData("mohamadreza.firooz@gmail.com")]
    public async Task Valid_Email_Should_Pass(string email)
    {
        var command = new CreateCustomerCommand("Reza", "Firooz", DateTime.Parse("3/24/1994"), "+989120259645", email, "BA177369567652933471");
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.IsValid.Should().BeTrue();
    }

    #endregion


    #region AccountNumber Validation
    [Fact]
    public async Task Default_String_AccountNumber_Should_Fail()
    {
        var command = new CreateCustomerCommand("Reza", "Firooz", DateTime.Parse("3/24/1994"), "+989120259645", "mohamadreza.firooz@gmail.com", "");
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[] { "BankAccountNumber is required.", "Invalid Bank Account Number. Please ensure correct mobile format." });
    }

    [Theory]
    [InlineData("Test")]
    public async Task Random_AccountNumber_Should_Fail(string accountNumber)
    {
        var command = new CreateCustomerCommand("Reza", "Firooz", DateTime.Parse("3/24/1994"), "+989120259645", "mohamadreza.firooz@gmail.com", accountNumber);
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]  { "Invalid Bank Account Number. Please ensure correct mobile format.",});
    }

    #endregion


    #region All Validation
    [Fact]
    public async Task Test_Command_Should_Pass()
    {
        var validationResult = await _validator.ValidateAsync(new CreateCustomerCommand("Reza", "Firooz", DateTime.Parse("3/24/1994"), "+989120259645", "mohamadreza.firooz@gmail.com", "BA177369567652933471"));
        validationResult.IsValid.Should().BeTrue();
    }



    [Theory]
    [InlineData("", "", "", "", "", 8)]
    public async Task All_Empty_Should_Fail(string firstName, string lastName, string phoneNumber, string email, string accountNumber, int failureCount)
    {
        var command = new CreateCustomerCommand(firstName, lastName, DateTime.Parse("3/24/1994"), phoneNumber, email, accountNumber);
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.Errors.Count.Should().Be(failureCount);
    }
    #endregion

}