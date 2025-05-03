using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class DataController : Controller {

    /**
        The data controller uses the Authorize-annotation which prevents access from unauthorized users
    **/

    [HttpGet("data")]
    [Authorize]
    public IActionResult GetData (){
        var responseData = new { message = "This is info for only authorized users" };
        return Ok(responseData);
    }

}