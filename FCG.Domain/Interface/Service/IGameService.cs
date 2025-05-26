using FCG.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Domain.Interface.Service;

public interface IGameService
{
    Task<IEnumerable<GameModel>> GetAllAsync();

    Task<GameModel> GetById(int gameId);

    Task<IActionResult> Create(GameModel gameReq);

    Task<IActionResult> DeleteById(int gameId);
}
