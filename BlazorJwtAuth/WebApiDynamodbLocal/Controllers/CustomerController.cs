using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using WebApiDynamodbLocal.Constants;
using WebApiDynamodbLocal.Dto;
using WebApiDynamodbLocal.Entities.ECommerce;
using WebApiDynamodbLocal.Services.Interfaces;

namespace WebApiDynamodbLocal.Controllers;

[Route(ApiPath.Customer)]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(string userName)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(new GetCustomerDto
            {
                Customer = null,
                Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),
                Message = "Validation Error",
                Succeeded = false
            });
        var response = await _customerService.GetByUserNameAsync(userName);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(PostCustomerDto postCustomerDto)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(new ResponseBaseDto
            {
                Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),
                Message = "Validation Error",
                Succeeded = false
            });
        var customer = new Customer
        {
            Type = Customer.EntityName,
            UserName = postCustomerDto.UserName,
            Email = postCustomerDto.Email,
            Name = postCustomerDto.Name,
            Addresses = postCustomerDto.Addresses
        };
        var response = await _customerService.CreateAsync(customer);
        if (!response.Succeeded) return UnprocessableEntity(response);
        return CreatedAtAction("Post", new {pk = customer.Key().Pk}, response);
    }
}
