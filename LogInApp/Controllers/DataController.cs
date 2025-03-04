using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class DataController : Controller {

    [HttpGet("data")]
    [Authorize]
    public IActionResult GetData (){
        var responseData = new { message = "This is info for only authorized users" };
        return Ok(responseData);
    }

}