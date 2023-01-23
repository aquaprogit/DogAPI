using Microsoft.AspNetCore.Mvc;

namespace DogAPI.Controllers;
[ApiController]
[Route("/")]
public class CommonController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("Dogs house service. Version 1.0.1");
    }
}
