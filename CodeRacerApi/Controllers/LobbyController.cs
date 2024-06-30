using System.Security.Claims;
using CodeRacerApi.Models;
using CodeRacerApi.Models.Lobby;
using CodeRacerApi.Models.SQLite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeRacerApi.Controllers;

[Authorize]
[Route("/api/[controller]")]
public class LobbyController : ControllerBase
{
    private readonly AppDbContext _context;

    public LobbyController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetLobbies()
    {
        var lobbies = await _context.Lobbys
            .Include(l => l.Users)
            .ToListAsync();

        var lobbyResponses = lobbies.Select(l => new LobbyResponse
        {
            Id = l.Id,
            Name = l.LobbyName,
            Users = l.Users.Select(u => u.Username).ToList()
        });
        
        return Ok(lobbyResponses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLobby(int id)
    {
        var lobby = await _context.Lobbys
            .Include(l => l.Users)
            .FirstOrDefaultAsync(l => l.Id == id);
        
        if (lobby == null)
        {
            return NotFound();
        }

        var lobbyResponse = new LobbyResponse
        {
            Id = lobby.Id,
            Name = lobby.LobbyName,
            Users = lobby.Users.Select(u => u.Username).ToList()
        };
        
        return Ok(lobbyResponse);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLobby(LobbyRequest lobby)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        _context.Add(new Lobby()
        {
            LobbyName = lobby.Id
        });
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("Join")]
    public async Task<IActionResult> JoinLobby(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var lobby = _context.Lobbys.FirstOrDefault(l => l.Id == id);
        if (lobby == null) return BadRequest();
        var user = await GetUserFromCookie(HttpContext);
        if (user == null) return BadRequest();
        lobby.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok();
    }

    private Task<User?> GetUserFromCookie(HttpContext httpContext)
    {
        var id = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
}