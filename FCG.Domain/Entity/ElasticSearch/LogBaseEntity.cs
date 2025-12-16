namespace FCG.Domain.Entity.ElasticSearch;

public class LogBaseEntity(string id, string name)
{
    public string Id { get; private set; } = id;

    public string Name { get; private set; } = name;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
}
