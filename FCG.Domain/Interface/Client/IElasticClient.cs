using FCG.Domain.Entity.ElasticSearch;

namespace FCG.Domain.Interface.Client;

public interface IElasticClient<T> where T : LogBaseEntity
{
    Task<IReadOnlyCollection<T>> GetLogs(int page, int size);

    Task<bool> Create(T log);
}
