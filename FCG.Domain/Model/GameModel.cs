namespace FCG.Domain.Model;

public class GameModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public decimal Value { get; set; }
}
