using System.Security.Claims;
using CodeRacerApi.Models;
using CodeRacerApi.Models.SQLite;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeRacerApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet]
    public IActionResult GetUser()
    {
        var name = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        var id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //TODO: check if user exists in database, if not create
        return Ok(new { user = name });
    }

    [HttpPost]
    public async Task<IActionResult> Login(string? name)
    {
        //TODO: Rate limit requests
        //Check Auth
        //Check if already has cookie.
        //// if (!ModelState.IsValid)
        // {
        //     return BadRequest(ModelState);
        // }

        //Create cookie with GUID to store anonymous user.
        var user = string.IsNullOrEmpty(name) ? "Guest" : name;
        var guid = Guid.NewGuid().ToString();
        //Check if guid is in db. If exist generate new.
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(
            new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user),
                new Claim(ClaimTypes.NameIdentifier, guid),
                new Claim(ClaimTypes.Anonymous, "true")
            }, CookieAuthenticationDefaults.AuthenticationScheme)
        ));
        var exists = await _context.Users.AnyAsync(u => u.Id == guid);
        if (exists) return BadRequest();
        
        await _context.AddAsync(new User
        {
            Id = guid,
            Username = user
        });

        await _context.SaveChangesAsync();
        return Ok(new { User = user });

    }
}