using GameStore.Data;
using GameStore.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connString);
// builder.Services.AddScoped<GameStoreContext>;

var app = builder.Build();

// all of the game data and game CRUD routes/endpoints and controller functions live here
app.MapGamesEndpoints();
app.MapGenresEndpoints();
// this extends our web application object
await app.MigrateDbAsync();

app.Run();

// root route rendering "Hello World!" text
// app.MapGet("/", () => "Hello World!");

 