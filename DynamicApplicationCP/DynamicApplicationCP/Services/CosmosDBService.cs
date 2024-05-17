using Microsoft.Azure.Cosmos;

namespace DynamicApplicationCP.Services
{
    public class CosmosDBService
    {
        private IConfiguration _configuration;
        private CosmosClient _cosmosClient;
        public CosmosDBService(IConfiguration configuration)
        {
            _configuration = configuration;
            string connectionString = _configuration?.GetConnectionString("CosmosDB");
            _cosmosClient = new CosmosClient(connectionString);
        }

        public async Task CreateOrUpdateItemAsync<T>(T item, string partitionKey, string databaseName, string containerName)
        {
            Container container = _cosmosClient.GetContainer(databaseName, containerName);
            await container.UpsertItemAsync(item, new PartitionKey(partitionKey));
        }

        public async Task<List<T>> GetDocumentsAsync<T>(string query, string databaseName, string containerName)
        {
            var queryDefinition = new QueryDefinition(query);
            Container container = _cosmosClient.GetContainer(databaseName, containerName);
            var queryIterator = container.GetItemQueryIterator<T>(queryDefinition);
            var results = new List<T>();

            while (queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task<string> DeleteDocumentAsync(string documentId, string databaseName, string containerName)
        {
            try
            {
                // Delete document
                Container container = _cosmosClient.GetContainer(databaseName, containerName);
                await container.DeleteItemAsync<dynamic>(documentId, new PartitionKey(documentId));
                return $"Deleted document with ID: {documentId}";
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return $"Document with ID: {documentId} not found";
            }
        }
    }
}
