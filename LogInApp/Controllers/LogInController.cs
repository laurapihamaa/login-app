using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LogInApp.Controllers;

[ApiController]
public class LogInController : Controller
{

    private readonly LogInService _loginService;
    private readonly IConfiguration _configuration;

    public LogInController(LogInService loginService, IConfiguration configuration)
    {
        _loginService = loginService;
        _configuration = configuration;
    }

    [HttpPost("login-user")]
    public async Task<IActionResult> LogInUser([FromQuery] string username, [FromQuery] string password)
    {
        try{

            bool loginOk = await _loginService.loginUser(username, password);

            if (loginOk){
                var token = GenerateJwtToken(username);


            var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddHours(1)
                };

                Response.Cookies.Append("jwt", token, cookieOptions);
                return Ok();
            }

            return Unauthorized();

        }catch (Exception ex){

            Console.WriteLine(ex.Message);

            return BadRequest("Request failed. Please try again.");

        }
    }

    [HttpPost("register-user")]
    public async Task<IActionResult> RegisterUser([FromQuery] string username, [FromQuery] string password){

        Regex regex = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
        Match match = regex.Match(password);

        if(!match.Success){
            return BadRequest("Invalid password format. Password should include at least one uppercase letter, one lowercase letter, one number"
                + " and one of special characters (#?!@$%^&*-)");
        }

        try
        {
            bool registerOk = await _loginService.registerUser(username, password);

            if (registerOk){
                var token = GenerateJwtToken(username);


            var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddHours(1)
                };

                Response.Cookies.Append("jwt", token, cookieOptions);
                return Ok();
            }

            return Unauthorized();

        }
        catch (Exception)
        {
            return BadRequest("Request failed. Please try again.");;
        }
    }

    private string GenerateJwtToken(string username){
        try{
            var claims = new []
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);

        }catch(Exception e){
            Console.WriteLine(e.Message);
            throw new Exception("Jwt token creation failed");
        }
    }
}
