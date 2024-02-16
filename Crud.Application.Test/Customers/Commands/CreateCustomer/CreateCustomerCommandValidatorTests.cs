

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
    private readonly ICollection<Customer> _customers = new List<Customer>
        {
            new()
            {
                Id = 1,
                FirstName = "Reza",
                LastName = "Tehran",
                DateOfBirth = DateTime.Parse("9/3/1990"),
                PhoneNumber = "+864473657383",
                Email = "egreenhow0@oaic.au",
                BankAccountNumber = "LT411593463320934097"
            },
            new()
            {
                Id = 2,
                FirstName = "Mari",
                LastName = "Shayan",
                DateOfBirth = DateTime.Parse("2/7/2000"),
                PhoneNumber = "+639963187652",
                Email = "ksherrin1@surveymonkey.com",
                BankAccountNumber = "TR3305582GTASOWMCZ0UMYHIU2"
            },
            new()
            {
                Id = 3,
                FirstName = "Winifred",
                LastName = "Ranvoise",
                DateOfBirth = DateTime.Parse("8/23/1989"),
                PhoneNumber = "+62 264 4398525",
                Email = "wranvoise2@virginia.edu",
                BankAccountNumber = "GE84LW8259319842791322"
            }
        };

    private readonly IApplicationDbContext _context;

    private readonly CreateCustomerCommandValidator _validator;

    public CreateCustomerCommandValidatorTests()
    {
        var mockDbSet = _customers.AsQueryable().BuildMockDbSet();
        var mockDbContext = new Mock<IApplicationDbContext>();
        mockDbContext.Setup(x => x.Customers).Returns(mockDbSet.Object);
        _context = mockDbContext.Object;

        _validator = new CreateCustomerCommandValidator(_context);
    }


    [Fact]
    public async Task PreparedCommand_ShouldPass()
    {
        var validationResult = await _validator.ValidateAsync(new CreateCustomerCommand("Any", "Belt", DateTime.Parse("9/20/1988"), "+989029058590", "abelt9@wikispaces.com", "BA177369567652933471"));
        validationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task EmptyStringAsPhoneNumber_ShouldFail()
    {
        var command = new CreateCustomerCommand("Any", "Belt", DateTime.Parse("9/20/1988"), "", "abelt9@wikispaces.com", "BA177369567652933471");
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
        {
            ""
        });
    }

    [Theory]
    [InlineData("RandomString")]
    public async Task RandomStringAsPhoneNumber_ShouldFail(string phoneNumber)
    {
        var command = new CreateCustomerCommand("Any", "Belt", DateTime.Parse("9/20/1988"), phoneNumber, "abelt9@wikispaces.com", "BA177369567652933471");
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
        {
                ""
        });
    }

    [Theory]
    [InlineData("+31717805045")]
    public async Task FixedLineNumber_ShouldFail(string phoneNumber)
    {
        var command = new CreateCustomerCommand("Any", "Belt", DateTime.Parse("9/20/1988"), phoneNumber, "abelt9@wikispaces.com", "BA177369567652933471");
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
        {
                ""
        });
    }

    [Theory]
    [InlineData("+989029058590")]
    public async Task MobileNumber_ShouldPass(string phoneNumber)
    {
        var command = new CreateCustomerCommand("Any", "Belt", DateTime.Parse("9/20/1988"), phoneNumber, "abelt9@wikispaces.com", "BA177369567652933471");
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task EmptyStringAsEmail_ShouldFail()
    {
        var command = new CreateCustomerCommand("Any", "Belt", DateTime.Parse("9/20/1988"), "+989029058590", "", "BA177369567652933471");
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
        {
                "'Email' must not be empty.",
                "'Email' is not a valid email address."
            });
    }

    [Theory]
    [InlineData("RandomString")]
    public async Task RandomStringAsEmail_ShouldFail(string email)
    {
        var command = new CreateCustomerCommand("Any", "Belt", DateTime.Parse("9/20/1988"), "+989029058590", email, "BA177369567652933471");
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
        {
                "'Email' is not a valid email address."
            });
    }

    [Theory]
    [InlineData("egreenhow0@oaic.gov.au")]
    public async Task PreviouslyUsedEmail_ShouldFail(string email)
    {
        var command = new CreateCustomerCommand("Any", "Belt", DateTime.Parse("9/20/1988"), "+989029058590", email, "BA177369567652933471");
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
        {
            ""
        });
    }

    [Theory]
    [InlineData("abelt9@wikispaces.com")]
    public async Task CorrectAndUnusedEmail_ShouldPass(string email)
    {
        var command = new CreateCustomerCommand("Any", "Belt", DateTime.Parse("9/20/1988"), "+989029058590", email, "BA177369567652933471");
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task EmptyStringAsAccountNumber_ShouldFail()
    {
        var command = new CreateCustomerCommand("Any", "Belt", DateTime.Parse("9/20/1988"), "+989029058590", "abelt9@wikispaces.com", "");
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
        {
                "'Bank Account Number' must not be empty.",
                ""
        });
    }

    [Theory]
    [InlineData("RandomString")]
    public async Task RandomStringAsAccountNumber_ShouldFail(string accountNumber)
    {
        var command = new CreateCustomerCommand("Any", "Belt", DateTime.Parse("9/20/1988"), "+989029058590", "abelt9@wikispaces.com", accountNumber);
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
        {
                "",
        });
    }

    [Theory]
    [InlineData("BA177369567652933471")]
    public async Task IbanAsAccountNumber_ShouldFail(string accountNumber)
    {
        var command = new CreateCustomerCommand("Any", "Belt", DateTime.Parse("9/20/1988"), "+989029058590", "abelt9@wikispaces.com", accountNumber);
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "", "", 6)]
    public async Task MultipleInvalidProperties_ShouldAccumulatesErrors(
        string phoneNumber, string email, string accountNumber, int failureCount)
    {
        var command = new CreateCustomerCommand("Any", "Belt", DateTime.Parse("9/20/1988"), phoneNumber, email, accountNumber);
        var validationResult = await _validator.ValidateAsync(command);
        validationResult.Errors.Count.Should().Be(failureCount);
    }
}