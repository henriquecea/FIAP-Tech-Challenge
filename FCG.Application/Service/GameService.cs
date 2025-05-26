using FCG.Domain.Interface.Service;
using FCG.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FCG.Application.Service;

public class GameService : IGameService
{
    private readonly ILogger<GameService> _logger;

    public GameService(ILogger<GameService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<IActionResult> Create(GameModel gameReq)
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> DeleteById(int gameId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<GameModel>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<GameModel> GetById(int gameId)
    {
        throw new NotImplementedException();
    }
}
