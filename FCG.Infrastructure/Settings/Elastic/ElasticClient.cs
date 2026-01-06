using Elastic.Clients.Elasticsearch;
using FCG.Domain.Entity.ElasticSearch;
using FCG.Domain.Interface.Client;

namespace FCG.Infrastructure.Settings.Elastic;

public class ElasticClient<T>(ElasticsearchClient client)
    : IElasticClient<T> where T : LogBaseEntity
{
    private static readonly IndexName Index =
        typeof(T).Name.ToLowerInvariant();

    public async Task<IReadOnlyCollection<T>> GetLogs(int page, int size)
    {
        var response = await client.SearchAsync<T>(s => s
            .Indices(Index)
            .From((page - 1) * size)
            .Size(size)
        );

        return response.Documents;
    }

    public async Task<bool> Create(T log)
    {
        var response = await client.IndexAsync(log, i => i.Index(Index));

        if (!response.IsValidResponse)
            return false;

        return true;
    }
}
