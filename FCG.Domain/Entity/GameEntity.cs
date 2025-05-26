namespace FCG.Domain.Entity;

public class GameEntity : BaseEntity
{
    public string Name { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public decimal Value { get; set; }
}
