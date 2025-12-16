using FCG.Domain.Interface.Settings;

namespace FCG.Infrastructure.Settings.Elastic;

public class ElasticSettings : IElasticSettings
{
    public string Uri => "http://localhost:9200";
}
