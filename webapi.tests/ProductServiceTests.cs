using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using FakeItEasy;
using FluentAssertions;

public class ProductServiceTests
{
    private ProductService _productService; // System Under Test (SUT)
    private IProductRepository _productRepository; // Mock
    private const string OnSaleKey = "b";
    private const int LowInventoryCount = 3;

    [SetUp]
    public void Setup()
    {
        var loggerFactory = A.Fake<LoggerFactory>();
        _productRepository = A.Fake<IProductRepository>();

        _productService = new ProductService(_productRepository, loggerFactory);
    }

    [Test]
    public void ShouldNotReturnProductsOnSale()
    {
        // Arrange (Given)
        A.CallTo(() => _productRepository.GetAllProducts()).Returns(
            new List<Product> {
                new Product {
                    Name = "product1",
                    Price = 1.99m
                },
                new Product {
                    Name = "product2",
                    Price = 1.99m
                }
            }
        );

        // Act (When)
        var productViewModels = _productService.GetAllProducts();

        // Assert (NUnit Assertions) (Then)
        Assert.That(productViewModels.Any(pdto => pdto.OnSale), Is.EqualTo(false));

        // Assert (FluentAssertions) (Then)
        productViewModels.Any(pdto => pdto.OnSale).Should().BeFalse();
    }

    [Test]
    public void ShouldReturnProductsOnSale()
    {
        // Arrange
        var expectedProductName = OnSaleKey + "test";

        A.CallTo(() => _productRepository.GetAllProducts()).Returns(
            new List<Product> {
                new Product {
                    Name = expectedProductName,
                    Price = 1.99m
                },
                new Product {
                    Name = "product2",
                    Price = 1.99m
                }
            }
        );

        // Act
        var productViewModels = _productService.GetAllProducts();

        // Assert (FluentAssertions)
        productViewModels.Count(pdto => pdto.OnSale).Should().Be(1);
        productViewModels.Single(pdto => pdto.OnSale).Name.Should().Be(expectedProductName);
    }

    [Test]
    public void ShouldNotReturnProductsWithLowInventory()
    {
        // Arrange
        A.CallTo(() => _productRepository.GetAllProducts()).Returns(
            new List<Product> {
                new Product {
                    Name = "product1",
                    Count = LowInventoryCount + 1
                },
                new Product {
                    Name = "product2",
                    Count = LowInventoryCount + 1
                }
            }
        );

        // Act
        var productViewModels = _productService.GetAllProducts();

        // Assert (FluentAssertions)
        productViewModels.Any(pdto => pdto.LowInventory).Should().BeFalse();
    }

    [Test]
    public void ShouldReturnProductsWithLowInventory()
    {
        // Arrange
        A.CallTo(() => _productRepository.GetAllProducts()).Returns(
            new List<Product> {
                new Product {
                    Name = "bike",
                    Count = 4
                },
                new Product {
                    Name = "product2",
                    Count = LowInventoryCount - 1
                }
            }
        );

        // Act
        var productViewModels = _productService.GetAllProducts();

        // Assert (FluentAssertions)
        productViewModels.Single(pdto => pdto.LowInventory).Name.Should().Be("product2");
    }
}