namespace Interview.Domain.Entities;

public class Hero
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string AlterEgo { get; set; } = string.Empty;
    public int Strength { get; set; }
    public int Speed { get; set; }
    public int Intelligence { get; set; }
    public int Durability { get; set; }
    public int Energy { get; set; }

    public ICollection<MissionHero> MissionHeroes { get; set; } = new List<MissionHero>();
}
