using GameStore.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

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
app.MapGet("games/{GameId}", (int GameId) => games.Find(game => game.GameId == GameId));

// root route rendering "Hello World!" text
// app.MapGet("/", () => "Hello World!");

app.Run();
