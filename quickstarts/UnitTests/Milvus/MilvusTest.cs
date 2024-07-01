using Azure;
using Azure.AI.OpenAI;
using OpenAI.Embeddings;
using System.ClientModel;

namespace UnitTests.Milvus;

public class MilvusTest(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public async Task RunCreateCollectionAsync()
    {
        MilvusDbClient milvusDbClient = new("127.0.0.1", 19530);

        bool result = await milvusDbClient.CreateCollectionAsync("test");

        Assert.True(result);
    }

    [Fact]
    public async Task RunUpsertEntitiesAsync()
    {
        string collectionName = "test";

        MilvusDbClient milvusDbClient = new("127.0.0.1", 19530);

        ReadOnlyMemory<float> embeddings = await this.GetEmbeddingAsync("search1");

        IReadOnlyList<string>? ids = await milvusDbClient.UpsertEntitiesAsync(collectionName, Guid.NewGuid().ToString(), "searc1", "content1", embeddings);

        Assert.NotNull(ids);
        Assert.True(ids.Any());
    }

    private async Task<ReadOnlyMemory<float>> GetEmbeddingAsync(string search)
    {
        AzureOpenAIClient azureOpenAIClient = new AzureOpenAIClient(new Uri(TestConfiguration.AzureOpenAIEmbeddings.Endpoint), new AzureKeyCredential(TestConfiguration.AzureOpenAIEmbeddings.ApiKey));

        EmbeddingClient embeddingClient = azureOpenAIClient.GetEmbeddingClient(TestConfiguration.AzureOpenAIEmbeddings.DeploymentName);

        ClientResult<Embedding> embeddingClientResult = await embeddingClient.GenerateEmbeddingAsync(search);

        return embeddingClientResult.Value.Vector;
    }
}
