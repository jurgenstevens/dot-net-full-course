namespace GameStore.Dtos;

public record class GameSummaryDto(
  int GameId, 
  string Name, 
  string Genre,
  decimal Price, 
  DateOnly ReleaseDate
);
