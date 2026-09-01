using Interview.Domain.Entities;

namespace Interview.Infrastructure.Persistence;

/// <summary>Original fictional heroes used to pre-seed the demo database.</summary>
public static class HeroSeedData
{
    public static readonly IReadOnlyList<Hero> Heroes =
    [
        new Hero { Name = "Solstice", AlterEgo = "Mara Quinn", Strength = 40, Speed = 55, Intelligence = 70, Durability = 45, Energy = 90 },
        new Hero { Name = "Ember", AlterEgo = "Rafael Costa", Strength = 60, Speed = 50, Intelligence = 55, Durability = 65, Energy = 80 },
        new Hero { Name = "Gale", AlterEgo = "Priya Nakamura", Strength = 35, Speed = 95, Intelligence = 60, Durability = 40, Energy = 70 },
        new Hero { Name = "Boulder", AlterEgo = "Otis Green", Strength = 95, Speed = 25, Intelligence = 40, Durability = 95, Energy = 30 },
        new Hero { Name = "Nimbus", AlterEgo = "Wren Ashford", Strength = 30, Speed = 65, Intelligence = 85, Durability = 35, Energy = 75 },
        new Hero { Name = "Cinder", AlterEgo = "Talia Novak", Strength = 55, Speed = 60, Intelligence = 50, Durability = 55, Energy = 85 },
        new Hero { Name = "Riptide", AlterEgo = "Marcus Ellery", Strength = 65, Speed = 70, Intelligence = 45, Durability = 60, Energy = 55 },
        new Hero { Name = "Thornback", AlterEgo = "Sable Reyes", Strength = 80, Speed = 40, Intelligence = 50, Durability = 85, Energy = 35 },
        new Hero { Name = "Wraith", AlterEgo = "Idris Kahale", Strength = 45, Speed = 85, Intelligence = 75, Durability = 30, Energy = 60 },
        new Hero { Name = "Lumen", AlterEgo = "Delphine Voss", Strength = 25, Speed = 55, Intelligence = 95, Durability = 30, Energy = 95 },
        new Hero { Name = "Ironclad", AlterEgo = "Bastian Kruger", Strength = 90, Speed = 35, Intelligence = 45, Durability = 90, Energy = 40 },
        new Hero { Name = "Nightshade", AlterEgo = "Aveline Storm", Strength = 50, Speed = 80, Intelligence = 65, Durability = 45, Energy = 65 },
    ];
}
