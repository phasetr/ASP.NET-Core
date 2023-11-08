using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using WebApiDynamodbLocal.Constants.ECommerce;
using WebApiDynamodbLocal.Dto.ECommerce.Customer;
using WebApiDynamodbLocal.Entities.ECommerce;
using WebApiDynamodbLocal.Services.ECommerce.Interfaces;

namespace WebApiDynamodbLocal.Controllers.ECommerce;

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
            return UnprocessableEntity(new GetCustomerResponseDto
            {
                CustomerModel = null,
                Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),
                Message = "Validation Error",
                Succeeded = false
            });
        var response = await _customerService.GetByUserNameAsync(userName);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(PostCustomerResponseDto postCustomerResponseDto)
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
            Type = nameof(Customer),
            UserName = postCustomerResponseDto.UserName,
            Email = postCustomerResponseDto.Email,
            Name = postCustomerResponseDto.Name,
            Addresses = postCustomerResponseDto.Addresses
        };
        var response = await _customerService.CreateAsync(customer);
        if (!response.Succeeded) return UnprocessableEntity(response);
        return CreatedAtAction("Post", new {pk = Key.CustomerPk(customer.UserName)}, response);
    }

    [HttpDelete(ApiPath.CustomerAddress)]
    public async Task<IActionResult> DeleteAddressAsync(string userName, string addressName)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(new DeleteAddressResponseDto
            {
                UserName = userName,
                AddressName = addressName,
                Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),
                Message = "Validation Error",
                Succeeded = false
            });
        var response = await _customerService.DeleteAddressAsync(userName, addressName);
        if (!response.Succeeded) return UnprocessableEntity(response);
        return Ok(response);
    }

    [HttpPut(ApiPath.CustomerAddress)]
    public async Task<IActionResult> PutAddressAsync(PutAddressDto dto)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(new ResponseBaseDto
            {
                Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),
                Message = "Validation Error",
                Succeeded = false
            });
        var response = await _customerService.PutAddressAsync(dto);
        if (!response.Succeeded) return UnprocessableEntity(response);
        return Ok(response);
    }
}
