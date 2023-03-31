using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApp.Models;

namespace ProductApp.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var products = new List<Product>
            {
                new Product { Id = 1,Name="Kalem",Price=10},
                 new Product { Id = 2,Name="Kitap",Price=130},
                  new Product { Id = 3,Name="Defter",Price=120}
     
            };
            _logger.LogInformation("Fetched products");
            return Ok(products);
        }

        [HttpPost]
        public IActionResult Add(Product product)
        {
            _logger.LogWarning("Product has been created.");
            return StatusCode(201);
        }
    }
}
