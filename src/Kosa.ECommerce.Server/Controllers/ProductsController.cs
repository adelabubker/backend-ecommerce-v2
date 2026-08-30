using Kosa.ECommerce.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kosa.ECommerce.Server.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(
        IProductService productService,
        ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting all products.");

        var result = await _productService.GetProductsAsync(
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchProducts(
        [FromQuery] string name,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Searching products by name: {ProductName}.",
            name);

        var result = await _productService.SearchProductsByNameAsync(
            name,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("category/{categoryId:int}")]
    public async Task<IActionResult> GetProductsByCategory(
        int categoryId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting products for category {CategoryId}.",
            categoryId);

        var result = await _productService.GetProductsByCategoryIdAsync(
            categoryId,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProductById(
        int id,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting product with ID {ProductId}.",
            id);

        var result = await _productService.GetProductByIdAsync(
            id,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetProductBySlug(
        string slug,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting product with slug {ProductSlug}.",
            slug);

        var result = await _productService.GetProductBySlugAsync(
            slug,
            cancellationToken);

        return Ok(result);
    }
}