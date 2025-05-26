using FCG.Domain.Interface.Service;
using FCG.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public Task<IActionResult> Create(GameModel gameReq)
    {
        throw new NotImplementedException();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete()]
    public Task<IActionResult> DeleteById(int gameId)
    {
        throw new NotImplementedException();
    }

    [Authorize(Roles = "Admin, User")]
    [HttpGet()]
    public Task<IEnumerable<GameModel>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    [Authorize(Roles = "Admin, User")]
    [HttpGet("{gameId}")]
    public Task<GameModel> GetById(int gameId)
    {
        throw new NotImplementedException();
    }
}
