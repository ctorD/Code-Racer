using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeRacerApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UserController : ControllerBase
{
    public class LoginRequest
    {
        public string? Name { get; set; }
    }

    private const string AuthenticationScheme = "default";
    
    
    [Authorize]
    [HttpGet]
    public IActionResult GetUser()
    {
        var name = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // var user = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        return Ok(new { user = name });
    }

    [HttpPost]
    public async Task<IActionResult> Login(string? name)
    {
        //Check Auth
        //Check if already has cookie.
        //// if (!ModelState.IsValid)
        // {
        //     return BadRequest(ModelState);
        // }

        //Create cookie with GUID to store anonymous user.
        var user = string.IsNullOrEmpty(name) ? Guid.NewGuid().ToString() : name;
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(
            new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user),
                new Claim(ClaimTypes.Anonymous, "true")
            }, CookieAuthenticationDefaults.AuthenticationScheme)
        ));

        return Ok(new { User = user });
    }
}