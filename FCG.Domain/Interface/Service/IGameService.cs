using FCG.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Domain.Interface.Service;

public interface IGameService
{
    /// <summary>
    /// Retrieves all games asynchronously.
    /// </summary>
    /// <returns>A collection of all game models.</returns>
    Task<IEnumerable<GameModel>> GetAllAsync();

    /// <summary>
    /// Retrieves a game by its ID.
    /// </summary>
    /// <param name="gameId">The ID of the game to retrieve.</param>
    /// <returns>The game model with the specified ID.</returns>
    Task<GameModel> GetById(int gameId);

    /// <summary>
    /// Creates a new game.
    /// </summary>
    /// <param name="gameReq">The game data to create.</param>
    /// <returns>The action result of the creation process.</returns>
    Task<IActionResult> Create(GameModel gameReq);

    /// <summary>
    /// Deletes a game by its ID.
    /// </summary>
    /// <param name="gameId">The ID of the game to delete.</param>
    /// <returns>The action result of the deletion process.</returns>
    Task<IActionResult> DeleteById(int gameId);
}

