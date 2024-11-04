﻿using Application.Customers.Commands.Create;
using Application.Customers.Queries.GetById;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Results;

namespace Api.Controllers;
[Route("api/[controller]")]
public class CustomersController : ApiController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerCommand command)
    {
        var result = await Sender.Send(command);

        return result.Match(
            customer => CreatedAtAction(
                actionName: nameof(GetById),
                routeValues: new { customerId = customer.Id },
                value: customer),
          Problem);
    }

    [HttpGet("{customerId:guid}")]
    public async Task<IActionResult> GetById(Guid customerId)
    {
        var result = await Sender.Send(new GetCustomerByIdQuery(customerId));
        return result.Match(Ok, Problem);
    }
}