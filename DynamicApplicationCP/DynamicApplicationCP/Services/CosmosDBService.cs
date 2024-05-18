using Microsoft.Azure.Cosmos;

namespace DynamicApplicationCP.Services
{
    public class CosmosDBService
    {
        private readonly IConfiguration _configuration;
        private readonly CosmosClient _cosmosClient;

        public CosmosDBService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            string connectionString = _configuration.GetConnectionString("CosmosDB")
                ?? throw new ArgumentNullException("CosmosDB connection string is missing in appsettings.json");

            _cosmosClient = new CosmosClient(connectionString);
        }

        public async Task CreateOrUpdateItemAsync<T>(T item, string partitionKey, string databaseName, string containerName)
        {
            try
            {
                Container container = _cosmosClient.GetContainer(databaseName, containerName);
                await container.UpsertItemAsync(item, new PartitionKey(partitionKey));
            }
            catch (CosmosException ex)
            {
                throw new Exception($"Error creating or updating item: {ex.Message}");
            }
        }

        public async Task<List<T>> GetDocumentsAsync<T>(string query, string databaseName, string containerName)
        {
            try
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
            catch (CosmosException ex)
            {
                throw new Exception($"Error querying documents: {ex.Message}");
            }
        }

        public async Task<string> DeleteDocumentAsync(string documentId, string databaseName, string containerName)
        {
            try
            {
                Container container = _cosmosClient.GetContainer(databaseName, containerName);
                await container.DeleteItemAsync<dynamic>(documentId, new PartitionKey(documentId));
                return $"Deleted document with ID: {documentId}";
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return $"Document with ID: {documentId} not found";
            }
            catch (CosmosException ex)
            {
                throw new Exception($"Error deleting document: {ex.Message}");
            }
        }
    }
}
