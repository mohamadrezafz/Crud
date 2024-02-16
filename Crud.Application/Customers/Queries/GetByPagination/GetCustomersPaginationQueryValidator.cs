
using Crud.Application.Customers.Commands.CreateCustomer;
using Crud.Application.Customers.Queries.GetByPagination;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Crud.Application.Customers.Queries.GetByPaginationp;

public class GetCustomersPaginationQueryValidator : AbstractValidator<GetCustomersPaginationQuery>
{
    public GetCustomersPaginationQueryValidator(IStringLocalizer<CreateCustomerCommand> localizer)
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage(e => string.Format(localizer["PageValidation"]));

        RuleFor(x => x.Count)
            .GreaterThanOrEqualTo(1)
            .WithMessage(e => string.Format(localizer["CountPageValidation"]));
    }
}