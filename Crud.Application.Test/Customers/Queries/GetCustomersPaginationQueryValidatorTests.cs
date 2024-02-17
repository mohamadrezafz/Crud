

using Crud.Application.Customers.Queries.GetByPagination;
using Crud.Application.Customers.Queries.GetByPaginationp;
using FluentAssertions;

namespace Crud.Application.Tests.Customers.Queries;

public class GetCustomersPaginationQueryValidatorTests
{
    private readonly GetCustomersPaginationQueryValidator _validator;

    public GetCustomersPaginationQueryValidatorTests()
    {
        _validator = new GetCustomersPaginationQueryValidator();
    }

    [Theory]
    [InlineData(-10)]
    [InlineData(0)]
    public async Task Page_Less1_Should_Fail(int pageNumber)
    {
        var validationResult = await _validator.ValidateAsync(new GetCustomersPaginationQuery { Page = pageNumber,  Count = 1 });
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[] { "Page  at least greater than or equal to 1." });
    }


    [Theory]
    [InlineData(-10)]
    [InlineData(0)]
    public async Task Count_Less1_Should_Fail(int pageSize)
    {
        var validationResult = await _validator.ValidateAsync(new GetCustomersPaginationQuery { Page = 1, Count = pageSize});
        validationResult.Errors.Select(e => e.ErrorMessage).Should().BeEquivalentTo(new[]{  "Page Count at least greater than or equal to 1." });
    }

    [Fact]
    public async Task Valid_Page_And_Count_Should_Pass()
    {
        var validationResult = await _validator.ValidateAsync(new GetCustomersPaginationQuery {Page = 2,  Count = 2  });
        validationResult.IsValid.Should().BeTrue();
    }
}
