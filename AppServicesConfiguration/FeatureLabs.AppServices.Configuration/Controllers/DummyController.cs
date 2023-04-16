using FeatureLabs.AppServices.Configuration.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FeatureLabs.AppServices.Configuration.Controllers;

[ApiController]
[Route("[controller]")]
public class DummyController : ControllerBase
{
    private readonly IOptions<AzureB2COptions> _options;

    public DummyController(IOptions<AzureB2COptions> options)
    {
        _options = options;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return new OkObjectResult(new { azureAdB2C = _options.Value });
    }
}
