namespace Interview.Application.Heroes;

public record HeroDto(Guid Id, string Name, string AlterEgo, int Strength, int Speed, int Intelligence, int Durability, int Energy);
