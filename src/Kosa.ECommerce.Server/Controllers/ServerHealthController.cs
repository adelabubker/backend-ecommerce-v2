using Microsoft.AspNetCore.Mvc;

namespace Kosa.ECommerce.Server.Controllers;

[ApiController]
[Route("api/server")]
public sealed class ServerHealthController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new
        {
            service = "Kosa ECommerce Server Layer",
            status = "Running",
            timestamp = DateTime.UtcNow
        });
    }
}
