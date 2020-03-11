using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger _logger;

        public ProductController(IProductService productService, ILoggerFactory loggerFactory)
        {
            _productService = productService;
            _logger = loggerFactory.CreateLogger("Controllers.ProductController");
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<Product>> GetAllProducts()
        {
            _logger.LogDebug("Getting all products");

            return Ok(_productService.GetAllProducts());
        }

        [HttpGet("{productId}")]
        [Authorize]
        public ActionResult<Product> GetProduct(int productId)
        {
            var product = _productService.GetProductById(productId);

            if (product != null) {
                return Ok(product);
            } else {
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult<Product> AddProduct(Product product)
        {
            _productService.AddProduct(product);

            // return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);

            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status201Created);
        }

        [HttpPut("{productId}")]
        [Authorize]
        public ActionResult UpdateProduct(int productId, Product productUpdate)
        {
            productUpdate.ProductId = productId;
            _productService.UpdateProduct(productUpdate);

            return NoContent();
        }

        [HttpDelete("{productId}")]
        [Authorize]
        public ActionResult DeleteProduct(int productId)
        {
            _productService.DeleteProduct(productId);

            return Ok();
        }
    }
}