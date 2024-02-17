using Crud.Application.Customers.Queries.GetByPagination;
using Crud.Application.Resources;
using FluentValidation;

namespace Crud.Application.Customers.Queries.GetByPaginationp;

/// <summary>
/// Validator for GetCustomersPaginationQuery to ensure the validity of pagination parameters.
/// </summary>
public class GetCustomersPaginationQueryValidator : AbstractValidator<GetCustomersPaginationQuery>
{
    public GetCustomersPaginationQueryValidator()
    {
        // Validation rules for the Page property
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage(Messages.PageValidation);

        // Validation rules for the Count property
        RuleFor(x => x.Count)
            .GreaterThanOrEqualTo(1)
            .WithMessage(Messages.CountPageValidation);
    }
}