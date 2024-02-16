
using Crud.Application.Common.Exceptions;
using FluentAssertions;
using FluentValidation.Results;

namespace Crud.Application.Tests.Common.Exceptions;

public class ValidationExceptionTests
{

    [Fact]
    public void DefaultConstructorCreatesAnEmptyErrorDictionary()
    {
        var actual = new ValidationException().Errors;

        actual.Keys.Should().BeEquivalentTo(Array.Empty<string>());
    }
    [Fact]
    public void ValidationFailureCreatesASingleElementErrorDictionary()
    {
        var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Email", "Invalid Email"),
            };

        var actual = new ValidationException(failures).Errors;

        actual.Keys.Should().BeEquivalentTo(new string[] { "Email" });
        actual["Email"].Should().BeEquivalentTo(new string[] { "Invalid Email" });
    }

    [Fact]
    public void MulitpleValidationFailureForMultiplePropertiesCreatesAMultipleElementErrorDictionaryEachWithMultipleValues()
    {
        var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Email", "Invalid Email"),
                new ValidationFailure("FullName", "FullName is required."),
                new ValidationFailure("FullName", "FullName must be less than 50 characters."),

            };

        var actual = new ValidationException(failures).Errors;

        actual.Keys.Should().BeEquivalentTo(new string[] { "Email", "FullName" });

        actual["Email"].Should().BeEquivalentTo(new string[]
        {
                "Invalid Email"
        });

        actual["FullName"].Should().BeEquivalentTo(new string[]
        {
                "FullName is required.",
                "FullName must be less than 50 characters."
        });
    }

}
