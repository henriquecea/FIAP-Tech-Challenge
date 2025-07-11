namespace FCG.Domain.Entity;

public class GameEntity : BaseEntity
{
    public GameEntity(string name, string gender)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        if (string.IsNullOrWhiteSpace(gender))
            throw new ArgumentException("Gender is required.", nameof(gender));

        Name = name;
        Gender = gender;
    }

    public string Name { get; set; }

    public string Gender { get; set; }

    public decimal Value { get; set; }
}
