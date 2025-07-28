using Microsoft.AspNetCore.Mvc;

namespace BicepHelloWorld.Controllers;
[ApiController]
[Route("[controller]")]
public class HelloWorldController : ControllerBase
{
    [HttpGet(Name = "HelloWorld")]
    public IActionResult Get()
    {
        return Ok("Hello, Bicep Workshop!");
    }
}
