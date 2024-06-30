using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CodeRacerApi.Controllers;

[ApiController]
[Route("/api/Lobbies")]
public class LobbiesController : ControllerBase
{
    [HttpGet(Name = "GetTest")]
    public async Task<IActionResult> Test()
    {
        return Ok();
    }
}