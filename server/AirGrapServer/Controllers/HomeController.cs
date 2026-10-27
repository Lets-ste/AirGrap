using Microsoft.AspNetCore.Mvc;
namespace AirGrap.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    private readonly IDbService _DbServise;

    public HomeController(IDbService DbServise)
    {
        _DbServise = DbServise;
    }
    [HttpGet("/")]
    public IActionResult Index()
    {
        return Ok("Server is Actiive");
    }
    [HttpGet("/test")]
    public async Task<IActionResult> TestConect()
    {
        return Ok((await _DbServise.CheckConnectionAsync()).ToString());
    }
}