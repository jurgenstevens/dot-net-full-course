using GameStore.Data;
using GameStore.Dtos;
using GameStore.Entities;
using GameStore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Endpoints;

public static class GamesEndpoints
{
  const string GetGameEndpointName = "GetGame";

  public static WebApplication MapGamesEndpoints(this WebApplication app)
  {
    // can't return group, but practicing using route groups for future projects
    var group = app.MapGroup("games").WithParameterValidation();

    // GET http://localhost:5202/games index route for games in JSON format
    group.MapGet("/", async (GameStoreContext dbContext) => 
        await dbContext.Games
                  .Include(game => game.Genre)
                  .Select(game => game.ToGameSummaryDto())
                  .AsNoTracking()
                  .ToListAsync());

    // GET http://localhost:5202/games/:gameId
    group.MapGet("/{gameId}", async (int gameId, GameStoreContext dbContext) => {
      Game? game = await dbContext.Games.FindAsync(gameId);
      return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
    })
    .WithName(GetGameEndpointName);

    // POST http://localhost:5202/games/
    group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) => 
    {
      Game game = newGame.ToEntity();
      // game.Genre = dbContext.Genres.Find(newGame.GenreId);

      dbContext.Games.Add(game);
      await dbContext.SaveChangesAsync();

      return Results.CreatedAtRoute(
        GetGameEndpointName, 
        new { GameId = game.Id }, 
        game.ToGameDetailsDto());
    });

    // PUT http://localhost:5202/games/:gameId
    group.MapPut("/{gameId}", async (int gameId, UpdateGameDto updatedGame, GameStoreContext dbContext) => 
    {
      var existingGame = await dbContext.Games.FindAsync(gameId);
      if (existingGame is null){
          return Results.NotFound();
      };

      dbContext.Entry(existingGame).CurrentValues.SetValues(updatedGame.ToEntity(gameId));
      await dbContext.SaveChangesAsync();

      return Results.NoContent();
    });

    // DELETE http://localhost:5202/games/:gameId
    group.MapDelete("/{gameId}", async (int gameId, GameStoreContext dbContext) => 
    {
      await dbContext.Games.Where(game => game.Id == gameId).ExecuteDeleteAsync();
      return Results.NoContent();
    });

    return app;  
  }
}
