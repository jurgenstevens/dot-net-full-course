using GameStore.Dtos;

namespace GameStore.Endpoints;

public static class GamesEndpoints
{
  const string GetGameEndpointName = "GetGame";

  private static readonly List<GameDto> games = [
    new (
        1, 
        "Street Fighter II", 
        "Fighting", 
        19.99M, 
        new DateOnly(1992, 7, 15)),
    new (
        2, 
        "Final Fantasy XIV",  
        "Roleplaying", 
        59.99M, 
        new DateOnly(2010, 9, 30)
        ),
    new (
        3, 
        "FIFA 23", 
        "Sports", 
        69.99M, 
        new DateOnly(2022, 9, 27)
    ),
  ];

  public static WebApplication MapGamesEndpoints(this WebApplication app)
  {
    // can't return group, but practicing using route groups for future projects
    var group = app.MapGroup("games").WithParameterValidation();

    // GET http://localhost:5202/games index route for games in JSON format
    group.MapGet("/", () => games);

    // GET http://localhost:5202/games/:gameId
    group.MapGet("/{gameId}", (int gameId) => {
      GameDto? game = games.Find(game => game.GameId == gameId);
      return game is null ? Results.NotFound() : Results.Ok(game);
    })
    .WithName(GetGameEndpointName);

    // POST http://localhost:5202/games/
    group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) => 
    {
      // if(string.IsNullOrEmpty(newGame.Name)){
      //   return Results.BadRequest("Name is required.");
      // };

      Game game = new()
      {
        Name = newGame.Name,
        Genre = dbContext.Genre.Find(newGame.GenreId),
        GenreId = newGame.GenreId,
        Price = newGame.Price,
        ReleaseDate = newGame.ReleaseDate,
      }

      dbContext.Games.Add(game);
      dbContext.SaveChanges();

      // GameDto game = new (
      //     games.Count + 1,
      //     newGame.Name,
      //     newGame.Genre,
      //     newGame.Price,
      //     newGame.ReleaseDate
      // );
      // games.Add(game);

      return Results.CreatedAtRoute(GetGameEndpointName, new { GameId = game.GameId}, game);
    });
    // .WithParameterValidation();

    // PUT http://localhost:5202/games/:gameId
    group.MapPut("/{gameId}", (int gameId, UpdateGameDto updatedGame) => 
    {
      var gameIndex = games.FindIndex(game => game.GameId == gameId);

      if(gameIndex == -1){
          return Results.NotFound();
      }

      games[gameIndex] = new GameDto(
          gameId,
          updatedGame.Name,
          updatedGame.Genre,
          updatedGame.Price,
          updatedGame.ReleaseDate
      ); 

      return Results.NoContent();
    });

    // DELETE http://localhost:5202/games/:gameId
    group.MapDelete("/{gameId}", (int gameId) => 
    {
      games.RemoveAll(game => game.GameId == gameId);
      return Results.NoContent();
    });

    return app;  
  }
}
