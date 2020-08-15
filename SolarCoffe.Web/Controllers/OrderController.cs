using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffe.Web.Serialization;
using SolarCoffe.Web.ViewModels;
using SolarCoffee.Services.Customer;
using SolarCoffee.Services.Order;

namespace SolarCoffe.Web.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService, ICustomerService customerService)
        {
            _logger = logger;
            _orderService = orderService;
            _customerService = customerService;
        }

        [HttpPost("/api/invoice")]
        public IActionResult GenerateNewOrder([FromBody ]InvoiceModel invoice){
            _logger.LogInformation("Generating Invoice.");
            var order = OrderMapper.SerilizationInvoiceToOrder(invoice);
            order.Customer = _customerService.GetById(invoice.CustomerId);
            _orderService.GenerateOpenOrder(order);
            return Ok();

        }
    }

}