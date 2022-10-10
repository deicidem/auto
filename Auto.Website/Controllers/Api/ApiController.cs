using Microsoft.AspNetCore.Mvc;

namespace Auto.Website.Controllers.Api;

[Route("api")]
[ApiController]
public class ApiController : ControllerBase {
    [HttpGet]
    public IActionResult Get() {
        var result = new {
            message = "Welcome to the Auto API!",
            _links = new {
                vehicles = new {
                    href = "/api/vehicles"
                },
                owners = new
                {
                    href = "/api/owners"
                }
            }
        };
        return new JsonResult(result);
    }
}