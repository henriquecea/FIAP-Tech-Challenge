namespace FCG.Domain.Configuration;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class ElasticIndexAttribute(string indexName) : Attribute
{
    public string IndexName { get; } = indexName;
}
