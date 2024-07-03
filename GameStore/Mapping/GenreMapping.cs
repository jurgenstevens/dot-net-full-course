using GameStore.Dtos;

namespace GameStore.Mapping;

public static class GenreMapping
{
  public static GenreDto ToDto(this GenreDto genre)
  {
    return new GenreDto(genre.Id, genre.Name);
  }

}
