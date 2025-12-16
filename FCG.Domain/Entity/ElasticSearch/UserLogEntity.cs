using FCG.Domain.Configuration;

namespace FCG.Domain.Entity.ElasticSearch;

[ElasticIndex("fiap-tech-user-logs")]
public class UserLogEntity(string id, string name) : LogBaseEntity(id, name)
{

}
