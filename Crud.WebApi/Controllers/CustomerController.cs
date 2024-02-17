using Crud.Application.Common.Models;
using Crud.Application.Customers.Commands.CreateCustomer;
using Crud.Application.Customers.Commands.DeleteCustomer;
using Crud.Application.Customers.Commands.UpdateCustomer;
using Crud.Application.Customers.Queries;
using Crud.Application.Customers.Queries.GetById;
using Crud.Application.Customers.Queries.GetByPagination;
using Crud.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Crud.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateCustomerCommand command)
    {
        var response = await Mediator.Send(command);
        return Created("", response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id ,[FromBody]UpdateCustomerCommand command)
    {
        if(id != command.Id) return BadRequest();
        await Mediator.Send(command);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteCustomerCommand(id));
        return Ok();
    }
    [HttpGet]
    public async Task<ActionResult<PaginationList<GetCustomersResponse>>> GetWithPagination([FromQuery] GetCustomersPaginationQuery query) => await Mediator.Send(query);


    [HttpGet("{id}")]
    public async Task<ActionResult<GetCustomersResponse>> Get(int id) => await Mediator.Send(new GetCustomerByIdQuery(id));
}
