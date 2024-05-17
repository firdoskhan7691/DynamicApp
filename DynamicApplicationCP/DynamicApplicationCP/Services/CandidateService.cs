using DynamicApplicationCP.Interfaces;
using DynamicApplicationCP.Models;

namespace DynamicApplicationCP.Services
{
    public class CandidateService : ICandidateService
    {
        private IConfiguration _configuration;
        private CosmosDBService _cosmosDBService;
        private string _cosmosDbName;
        private string _cosmosDbContainerName;
        public CandidateService(IConfiguration configuration, CosmosDBService cosmosDBService)
        {
            _configuration = configuration;
            _cosmosDBService = cosmosDBService;
            _cosmosDbName = _configuration["CosmosDB:DatabaseName"];
            _cosmosDbContainerName = _configuration["CosmosDB:CandidateContainerName"];
        }
        public async Task AddCandidateApplication(CandidateModel candidateModel)
        {
            await _cosmosDBService.CreateOrUpdateItemAsync(candidateModel, candidateModel.CandidateId, _cosmosDbName, _cosmosDbContainerName);
        }
    }
}
