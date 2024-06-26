using GameStore.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connString = "Data Source=GameStore.db";

var app = builder.Build();

// all of the game data and game CRUD routes/endpoints and controller functions live here
app.MapGamesEndpoints();

app.Run();

// root route rendering "Hello World!" text
// app.MapGet("/", () => "Hello World!");

 