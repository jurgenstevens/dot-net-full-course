using GameStore;
using GameStore.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
const string GetGameEndpointName = "GetGame";

List<GameDto> games = [
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

// GET http://localhost:5202/games index route for games in JSON format
app.MapGet("games", () => games);

// GET http://localhost:5202/games/:gameId
app.MapGet("games/{gameId}", (int gameId) => 
{
   GameDto? game = games.Find(game => game.GameId == gameId);
   return game is null ? Results.NotFound() : Results.Ok(game);
})
.WithName(GetGameEndpointName);

// POST http://localhost:5202/games/
app.MapPost("games", (CreateGameDto newGame) => {
   GameDto game = new(
      games.Count + 1,
      newGame.Name,
      newGame.Genre,
      newGame.Price,
      newGame.ReleaseDate
   );
   games.Add(game);
   return Results.CreatedAtRoute(GetGameEndpointName, new { GameId = game.GameId}, game);
});

// PUT http://localhost:5202/games/:gameId
app.MapPut("games/{gameId}", (int gameId, UpdateGameDto updatedGame) => 
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
app.MapDelete("games/{gameId}", (int gameId) =>
{
   games.RemoveAll(game => game.GameId == gameId);
   return Results.NoContent();
});


// root route rendering "Hello World!" text
// app.MapGet("/", () => "Hello World!");

app.Run();
