using System.Collections.Generic;
using Microsoft.Extensions.Logging;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRespository;
    private readonly ILogger _logger;

    public ProductService(IProductRepository productRepository, ILoggerFactory loggerFactory)
    {
        _productRespository = productRepository;
        _logger = loggerFactory.CreateLogger("Controllers.ProductService");
    }

    public List<ProductViewModel> GetAllProducts()
    {
        var productViewModels = new List<ProductViewModel>();

        foreach(var product in _productRespository.GetAllProducts())
        {
            productViewModels.Add(new ProductViewModel {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                OnSale = BusinessRules.isOnSale(product),
                LowInventory = BusinessRules.isLowInventory(product)
            });
            if (BusinessRules.isLowInventory(product))
            {
                _logger.LogInformation("Found low inventory product: " + product.ProductId);
            }
        }

        return productViewModels;
    }

    public Product GetProductById(long productId)
    {
        return _productRespository.GetProductById(productId);
    }

    public void AddProduct(Product product)
    {
        _productRespository.AddProduct(product);
    }

    public void UpdateProduct(Product product)
    {
        _productRespository.UpdateProduct(product);
    }

    public void DeleteProduct(long productId)
    {
        _productRespository.DeleteProduct(productId);
    }
}