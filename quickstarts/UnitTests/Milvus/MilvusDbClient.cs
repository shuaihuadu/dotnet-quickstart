using Milvus.Client;

namespace UnitTests.Milvus;

public class MilvusDbClient
{
    private readonly MilvusClient _milvusClient;

    const int maxLength = 65535;

    const string idField = "id";
    const string searchField = "search";
    const string contentField = "content";
    const string embeddingField = "embedding";

    public MilvusDbClient(string host, int port)
    {
        this._milvusClient = new MilvusClient(host, port);
    }

    /// <inheritdoc />
    public async Task<bool> CreateCollectionAsync(string collectionName, CancellationToken cancellationToken = default)
    {
        bool hasCollection = await this._milvusClient.HasCollectionAsync(collectionName, cancellationToken: cancellationToken);

        if (hasCollection)
        {
            return hasCollection;
        }

        CollectionSchema collectionSchema = new()
        {
            Name = collectionName,
            EnableDynamicFields = true
        };

        collectionSchema.Fields.Add(FieldSchema.CreateVarchar(idField, maxLength, true, false));
        collectionSchema.Fields.Add(FieldSchema.CreateVarchar(searchField, maxLength));
        collectionSchema.Fields.Add(FieldSchema.CreateVarchar(contentField, maxLength));
        collectionSchema.Fields.Add(FieldSchema.CreateFloatVector(embeddingField, 1536));

        MilvusCollection collection = await _milvusClient.CreateCollectionAsync(collectionName, collectionSchema, cancellationToken: cancellationToken).ConfigureAwait(false);

        //创建向量索引
        string embeddingIndexName = $"embedding_index";
        await collection.CreateIndexAsync(embeddingField, IndexType.AutoIndex, SimilarityMetricType.Cosine, indexName: embeddingIndexName, cancellationToken: cancellationToken).ConfigureAwait(false);
        await collection.WaitForIndexBuildAsync(embeddingField, indexName: embeddingIndexName, cancellationToken: cancellationToken).ConfigureAwait(false);

        await collection.LoadAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
        await collection.WaitForCollectionLoadAsync(waitingInterval: TimeSpan.FromMilliseconds(100), timeout: TimeSpan.FromMinutes(1), cancellationToken: cancellationToken).ConfigureAwait(false);

        return true;
    }

    public async Task<IReadOnlyList<string>?> UpsertEntitiesAsync(string collectionName, string id, string search, string content, ReadOnlyMemory<float> embedding)
    {
        List<FieldData> fieldDatas = [];
        fieldDatas.Add(FieldData.CreateVarChar(idField, [id]));
        fieldDatas.Add(FieldData.CreateVarChar(searchField, [search]));
        fieldDatas.Add(FieldData.CreateVarChar(contentField, [content]));
        fieldDatas.Add(FieldData.CreateFloatVector(embeddingField, [embedding]));

        MilvusCollection collection = this._milvusClient.GetCollection(collectionName);

        MutationResult insertResult = await collection.UpsertAsync(fieldDatas);

        return insertResult.Ids.StringIds;
    }
}