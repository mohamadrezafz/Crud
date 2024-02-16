

using Crud.Application.Common.Interfaces;
using Crud.Application.Customers.Commands.UpdateCustomer;
using Crud.Domain.Entities;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;

namespace Crud.Application.Tests.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandValidatorTests
{
    private readonly ICollection<Customer> _customers = new List<Customer>
        {
            new()
            {
                Id = 1,
                FirstName = "Ernst",
                LastName = "Greenhow",
                DateOfBirth = DateTime.Parse("9/3/1988"),
                PhoneNumber = "+86 447 365 7383",
                Email = "egreenhow0@oaic.gov.au",
                BankAccountNumber = "LT41 1593 4633 2093 4097"
            },
            new()
            {
                Id = 2,
                FirstName = "Kari",
                LastName = "Sherrin",
                DateOfBirth = DateTime.Parse("2/7/1999"),
                PhoneNumber = "+63 996 318 7652",
                Email = "ksherrin1@surveymonkey.com",
                BankAccountNumber = "TR33 0558 2GTA SOWM CZ0U MYHI U2"
            },
            new()
            {
                Id = 3,
                FirstName = "Winifred",
                LastName = "Ranvoise",
                DateOfBirth = DateTime.Parse("8/23/1989"),
                PhoneNumber = "+62 264 439 8525",
                Email = "wranvoise2@virginia.edu",
                BankAccountNumber = "GE84 LW82 5931 9842 7913 22"
            }
        };

    private readonly IApplicationDbContext _context;

    private readonly UpdateCustomerCommandValidator _validator;

    public UpdateCustomerCommandValidatorTests()
    {
        var mockDbSet = _customers.AsQueryable().BuildMockDbSet();
        var mockDbContext = new Mock<IApplicationDbContext>();
        mockDbContext.Setup(x => x.Customers).Returns(mockDbSet.Object);
        _context = mockDbContext.Object;

        _validator = new UpdateCustomerCommandValidator(_context);
    }

    [Fact]
    public async Task PreparedCommand_ShouldPass()
    {
        var validationResult = await _validator.ValidateAsync(new UpdateCustomerCommand(4, "Any", "Belt", DateTime.Parse("9/20/1988"), "+989029058590", "abelt9@wikispaces.com", "BA177369567652933471"));
        validationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task EmptyStringAsPhoneNumber_ShouldFail()
    {
        var validationResult = await _validator.ValidateAsync(new UpdateCustomerCommand(4, "Any", "Belt", DateTime.Parse("9/20/1988"), "", "abelt9@wikispaces.com", "BA177369567652933471"));
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
        {
                "'Phone Number' must not be empty.",
                ""
        });
    }

    [Theory]
    [InlineData("RandomString")]
    public async Task RandomStringAsPhoneNumber_ShouldFail(string phoneNumber)
    {
        var validationResult = await _validator.ValidateAsync(new UpdateCustomerCommand(4, "Any", "Belt", DateTime.Parse("9/20/1988"), phoneNumber, "abelt9@wikispaces.com", "BA177369567652933471"));
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
        {
                ""
        });
    }

    [Theory]
    [InlineData("+31717805045")]
    public async Task FixedLineNumber_ShouldFail(string phoneNumber)
    {
        var validationResult = await _validator.ValidateAsync(new UpdateCustomerCommand(4, "Any", "Belt", DateTime.Parse("9/20/1988"), phoneNumber, "abelt9@wikispaces.com", "BA177369567652933471"));
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
        {
                ""
        });
    }

    [Theory]
    [InlineData("+989029058590")]
    public async Task MobileNumber_ShouldPass(string phoneNumber)
    {
        var validationResult = await _validator.ValidateAsync(new UpdateCustomerCommand(4, "Any", "Belt", DateTime.Parse("9/20/1988"), phoneNumber, "abelt9@wikispaces.com", "BA177369567652933471"));
        validationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task EmptyStringAsEmail_ShouldFail()
    {
        var validationResult = await _validator.ValidateAsync(new UpdateCustomerCommand(4, "Any", "Belt", DateTime.Parse("9/20/1988"), "+989029058590", "", "BA177369567652933471"));
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
        var validationResult = await _validator.ValidateAsync(new UpdateCustomerCommand(4, "Any", "Belt", DateTime.Parse("9/20/1988"), "+989029058590", email, "BA177369567652933471"));
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
        {
                "'Email' is not a valid email address."
            });
    }

    [Theory]
    [InlineData("egreenhow0@oaic.gov.au")]
    public async Task PreviouslyUsedEmailByOthers_ShouldFail(string email)
    {
        var validationResult = await _validator.ValidateAsync(new UpdateCustomerCommand(4, "Any", "Belt", DateTime.Parse("9/20/1988"), "+989029058590", email, "BA177369567652933471"));
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
        {
                ""
        });
    }

    [Theory]
    [InlineData("egreenhow0@oaic.gov.au")]
    public async Task PreviouslyUsedEmailBySelf_ShouldPass(string email)
    {
        var validationResult = await _validator.ValidateAsync(new UpdateCustomerCommand(1, "Any", "Belt", DateTime.Parse("9/20/1988"), "+989029058590", email, "BA177369567652933471"));
        validationResult.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("abelt9@wikispaces.com")]
    public async Task CorrectAndUnusedEmail_ShouldPass(string email)
    {
        var validationResult = await _validator.ValidateAsync(new UpdateCustomerCommand(1, "Any", "Belt", DateTime.Parse("9/20/1988"), "+989029058590", email, "BA177369567652933471"));
        validationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task EmptyStringAsAccountNumber_ShouldFail()
    {
        var validationResult = await _validator.ValidateAsync(new UpdateCustomerCommand(1, "Any", "Belt", DateTime.Parse("9/20/1988"), "+989029058590", "abelt9@wikispaces.com", ""));
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
        {
                "'Bank Account Number' must not be empty.",
                "",
        });
    }

    [Theory]
    [InlineData("RandomString")]
    public async Task RandomStringAsAccountNumber_ShouldFail(string accountNumber)
    {
        var validationResult = await _validator.ValidateAsync(new UpdateCustomerCommand(1, "Any", "Belt", DateTime.Parse("9/20/1988"), "+989029058590", "abelt9@wikispaces.com", accountNumber));
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]
        {
                "",
        });
    }

    [Theory]
    [InlineData("BA177369567652933471")]
    public async Task IbanAsAccountNumber_ShouldFail(string accountNumber)
    {
        var validationResult = await _validator.ValidateAsync(new UpdateCustomerCommand(1, "Any", "Belt", DateTime.Parse("9/20/1988"), "+989029058590", "abelt9@wikispaces.com", accountNumber));
        validationResult.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "", "", 6)]
    public async Task MultipleInvalidProperties_ShouldAccumulatesErrors(
        string phoneNumber, string email, string accountNumber, int failureCount)
    {
        var validationResult = await _validator.ValidateAsync(new UpdateCustomerCommand(1, "Any", "Belt", DateTime.Parse("9/20/1988"), phoneNumber, email, accountNumber));
        validationResult.Errors.Count.Should().Be(failureCount);
    }
}
