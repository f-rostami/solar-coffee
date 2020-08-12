using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Services.Product;

namespace SolarCoffe.Web.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet("/api/product")]
        public IActionResult GetProduct()
        {
            _logger.LogInformation("Getting all products");
            var products = _productService.GetAllProducts();
            return Ok(products);
        }
    }
}