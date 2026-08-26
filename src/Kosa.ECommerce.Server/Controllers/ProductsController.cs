using Kosa.ECommerce.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kosa.ECommerce.Server.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
    {
        var result = await _productService.GetProductsAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchProducts([FromQuery] string name, CancellationToken cancellationToken)
    {
        var result = await _productService.SearchProductsByNameAsync(name, cancellationToken);
        return Ok(result);
    }

    [HttpGet("category/{categoryId:int}")]
    public async Task<IActionResult> GetProductsByCategory(int categoryId, CancellationToken cancellationToken)
    {
        var result = await _productService.GetProductsByCategoryIdAsync(categoryId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProductById(int id, CancellationToken cancellationToken)
    {
        var result = await _productService.GetProductByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetProductBySlug(string slug, CancellationToken cancellationToken)
    {
        var result = await _productService.GetProductBySlugAsync(slug, cancellationToken);
        return Ok(result);
    }
}
