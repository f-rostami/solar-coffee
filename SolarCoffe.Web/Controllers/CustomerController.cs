using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffe.Web.Serialization;
using SolarCoffe.Web.ViewModels;
using SolarCoffee.Services.Customer;

namespace SolarCoffe.Web.Controllers
{
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        [HttpPost("/api/customer")]
        public IActionResult CreateCustomer([FromBody] CustomerModel customer)
        {
            _logger.LogInformation("Creating a new customer");
            customer.CreatedOn = DateTime.UtcNow;
            customer.UpdatedOn = DateTime.UtcNow;
            var customerData = CustomerMapper.SerializeCustomer(customer);
            var newCustomer = _customerService.Create(customerData);
            return Ok(newCustomer);

        }

        [HttpGet("/api/customer")]
        public IActionResult GetCustomers()
        {
            _logger.LogInformation("Getting Customers");
            var customers = _customerService.GetAll();
            var customersModel = customers.Select(customer => new CustomerModel
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PrimaryAddress = CustomerMapper.MapCustomerAddress(customer.PrimaryAddress),
                CreatedOn = customer.CreatedOn,
                UpdatedOn = customer.UpdatedOn
            })
            .OrderByDescending(customer => customer.CreatedOn)
            .ToList();

            return Ok(customersModel);
        }

        [HttpDelete("/api/customer/{id}")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Deleting a customer");
            var response = _customerService.Delete(id);
            return Ok(response);
        }

    }
}