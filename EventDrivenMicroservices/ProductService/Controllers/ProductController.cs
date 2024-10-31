using Microsoft.AspNetCore.Mvc;
using ProductService.Models;
using ProductService.RabbitMQ;
using ProductService.Repositories;

namespace ProductService.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepo;
    private readonly ProductPublisher _publisher;

    public ProductController(IProductRepository productRepo, ProductPublisher publisher)
    {
        _productRepo = productRepo;
        _publisher = publisher;
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProductById(string id) =>
        await _productRepo.GetByIdAsync(id);

    [HttpPut("{id}/stock")]
    public async Task<IActionResult> UpdateStock(string id, int quantity)
    {
        await _productRepo.UpdateStockAsync(id, quantity);
        var product = await _productRepo.GetByIdAsync(id);
        _publisher.PublishProductUpdate(product);
        return Ok();
    }
}