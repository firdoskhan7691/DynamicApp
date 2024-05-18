using DynamicApplicationCP.Interfaces;
using DynamicApplicationCP.Models;

namespace DynamicApplicationCP.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly IConfiguration _configuration;
        private readonly ICosmosDBService _cosmosDBService;
        private readonly string _cosmosDbName;
        private readonly string _cosmosDbContainerName;

        public CandidateService(IConfiguration configuration, ICosmosDBService cosmosDBService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _cosmosDBService = cosmosDBService ?? throw new ArgumentNullException(nameof(cosmosDBService));

            _cosmosDbName = _configuration["CosmosDB:DatabaseName"]
                ?? throw new ArgumentNullException("CosmosDB:DatabaseName configuration is missing in appsettings.json");

            _cosmosDbContainerName = _configuration["CosmosDB:CandidateContainerName"]
                ?? throw new ArgumentNullException("CosmosDB:CandidateContainerName configuration is missing in appsettings.json");
        }

        public async Task AddCandidateApplication(CandidateModel candidateModel)
        {
            if (candidateModel == null)
            {
                throw new ArgumentNullException(nameof(candidateModel));
            }

            candidateModel.CandidateId = Guid.NewGuid().ToString();
            await _cosmosDBService.CreateOrUpdateItemAsync(candidateModel, candidateModel.CandidateId, _cosmosDbName, _cosmosDbContainerName);
        }
    }
}
