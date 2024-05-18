namespace DynamicApplicationCP.Interfaces
{
    public interface ICosmosDBService
    {
        Task CreateOrUpdateItemAsync<T>(T item, string partitionKey, string databaseName, string containerName);
        Task<List<T>> GetDocumentsAsync<T>(string query, string databaseName, string containerName);
        Task<string> DeleteDocumentAsync(string documentId, string databaseName, string containerName);

    }
}
