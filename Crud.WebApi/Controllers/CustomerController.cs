using Crud.Application.Customers.Commands.CreateCustomer;
using Crud.Application.Customers.Commands.DeleteCustomer;
using Crud.Application.Customers.Commands.UpdateCustomer;
using Crud.Application.Customers.Queries.GetById;
using Crud.Application.Customers.Queries.GetByPagination;
using Crud.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Crud.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ApiControllerBase
{
    // POST api/customers
    // Creates a new customer using the provided CreateCustomerCommand.
    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateCustomerCommand command)
    {
        var response = await Mediator.Send(command);
        return Created("", response); // Returns a 201 Created response with the created resource.
    }
    // PUT api/customers/{id}
    // Updates an existing customer with the specified ID using the UpdateCustomerCommand.
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id ,[FromBody]UpdateCustomerCommand command)
    {
        if(id != command.Id) return BadRequest();
        await Mediator.Send(command);
        return Ok();// Returns a 200 OK response.
    }
    // DELETE api/customers/{id}
    // Deletes the customer with the specified ID.
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteCustomerCommand(id));
        return Ok();// Returns a 200 OK response.
    }
    // GET api/customers
    // Retrieves a list of customers with pagination using the GetCustomersPaginationQuery.
    [HttpGet]
    public async Task<IActionResult> GetWithPagination([FromQuery] GetCustomersPaginationQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result); // Returns a 200 OK response with the paginated list of customers.
    }

    // GET api/customers/{id}
    // Retrieves a specific customer by ID.
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var customer = await Mediator.Send(new GetCustomerByIdQuery(id));

        if (customer == null)
            return NotFound();// Returns a 404 Not Found response if the customer is not found.


        return Ok(customer);// Returns a 200 OK response with the customer data.
    }
}
