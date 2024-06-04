namespace GameStore.Dtos;

public record class GameDto(
  int GameId, 
  string Name, 
  string Genre,
  decimal Price, 
  DateOnly ReleaseDate
);
