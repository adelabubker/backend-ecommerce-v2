using Microsoft.AspNetCore.Mvc;

namespace Kosa.ECommerce.Server.Controllers;

[ApiController]
[Route("api/server")]
public sealed class ServerHealthController : ControllerBase
{
    private readonly ILogger<ServerHealthController> _logger;

    public ServerHealthController(
        ILogger<ServerHealthController> logger)
    {
        _logger = logger;
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        _logger.LogInformation(
            "Health check requested for Kosa ECommerce Server.");

        return Ok(new
        {
            service = "Kosa ECommerce Server Layer",
            status = "Running",
            timestamp = DateTime.UtcNow
        });
    }
}