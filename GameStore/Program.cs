using GameStore.Data;
using GameStore.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connString);

var app = builder.Build();

// all of the game data and game CRUD routes/endpoints and controller functions live here
app.MapGamesEndpoints();
// this extends our web application object
app.MigrateDb();

app.Run();

// root route rendering "Hello World!" text
// app.MapGet("/", () => "Hello World!");

 