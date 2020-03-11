using System.Collections.Generic;

public interface IProductRepository
{
    List<Product> GetAllProducts();
    Product GetProductById(long productId);

    void AddProduct(Product product);

    void UpdateProduct(Product product);

    void DeleteProduct(long productId);
}