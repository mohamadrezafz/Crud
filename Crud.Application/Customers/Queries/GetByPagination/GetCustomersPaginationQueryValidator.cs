
using Crud.Application.Customers.Commands.CreateCustomer;
using Crud.Application.Customers.Queries.GetByPagination;
using Crud.Application.Resources;
using FluentValidation;

namespace Crud.Application.Customers.Queries.GetByPaginationp;

public class GetCustomersPaginationQueryValidator : AbstractValidator<GetCustomersPaginationQuery>
{
    public GetCustomersPaginationQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage(Messages.PageValidation);

        RuleFor(x => x.Count)
            .GreaterThanOrEqualTo(1)
            .WithMessage(Messages.CountPageValidation);
    }
}