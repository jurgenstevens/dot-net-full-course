﻿namespace GameStore.Dtos;

public record class GameDetailsDto(
  int GameId,
  string Name,
  int GenreId,
  decimal Price,
  DateOnly ReleaseDate
);
