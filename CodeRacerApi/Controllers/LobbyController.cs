using System.Security.Claims;
using CodeRacerApi.Models.Lobby;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeRacerApi.Controllers;

[Authorize]
public class LobbyController : ControllerBase
{
    [HttpGet]
    public IActionResult GetLobbies()
    {
        return Ok();
    }
    
    [HttpGet("GetLobby/{id}")]
    public IActionResult GetLobby()
    {
        return Ok();
    }
    
    [HttpPost]
    public IActionResult CreateLobby(LobbyRequest lobby)
    {
        return Ok();
    }
    
    
    
    
    
}