using System.Security.Claims;
using CodeRacerApi.Models;
using CodeRacerApi.Models.Lobby;
using CodeRacerApi.Models.SQLite;
using CodeRacerApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Octokit;
using User = CodeRacerApi.Models.SQLite.User;

namespace CodeRacerApi.Controllers;

[Authorize]
[Route("/api/[controller]")]
public class LobbyController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly GithubService _githubService;

    public LobbyController(AppDbContext context, GithubService githubService)
    {
        _context = context;
        _githubService = githubService;
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

        var snippet = await _githubService.GetSnippet(LangTextToEnum(lobby.Language));
        await _context.AddAsync(new Lobby()
        {
            LobbyName = lobby.Id,
            Snippet = snippet
        });
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("{id}")]
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

    private Language LangTextToEnum(string lang)
    {
        switch (lang)
        {
            case "js":
                return Language.JavaScript;
            case "ts":
                return Language.TypeScript;
            case "csharp":
                return Language.CSharp;
            default:
                return Language.JavaScript;
        }
    }
}