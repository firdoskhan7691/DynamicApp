using DynamicApplicationCP.Interfaces;
using DynamicApplicationCP.Models;

namespace DynamicApplicationCP.Services
{
    public class QuestionService : IQuestionService
    {
        private IConfiguration _configuration;
        private CosmosDBService _cosmosDBService;
        private string _cosmosDbName;
        private string _cosmosDbContainerName;
        public QuestionService(IConfiguration configuration, CosmosDBService cosmosDBService)
        {
            _configuration = configuration;
            _cosmosDBService = cosmosDBService;
            _cosmosDbName = _configuration["CosmosDB:DatabaseName"];
            _cosmosDbContainerName = _configuration["CosmosDB:QuestionContainerName"];
        }

        public async Task AddQuestionAsync(QuestionModel questionModel)
        {            
            await _cosmosDBService.CreateOrUpdateItemAsync(questionModel, questionModel.QuestionId, _cosmosDbName, _cosmosDbContainerName);
        }

        public async Task AddMultipleQuestionsAsync(List<QuestionModel> lstQuestionModel)
        {
            List<Task> concurrentTask = new List<Task>();

            foreach (var item in lstQuestionModel)
            {
                item.QuestionId = Guid.NewGuid().ToString();
                concurrentTask.Add(_cosmosDBService.CreateOrUpdateItemAsync(item, item.QuestionId, _cosmosDbName, _cosmosDbContainerName));
            }

            await Task.WhenAll(concurrentTask);
        }

        public async Task<string> DeleteQuestionByIdAsync(string questionId)
        {
            return await _cosmosDBService.DeleteDocumentAsync(questionId, _cosmosDbName, _cosmosDbContainerName);
        }

        public async Task<List<QuestionModel>> GetQuestionsByProgramIdAsync(string programId)
        {
            string query = $"SELECT * FROM c where c.programId = '{programId}'";

            List<QuestionModel> lstQuestionModel = await _cosmosDBService.GetDocumentsAsync<QuestionModel>(query, _cosmosDbName, _cosmosDbContainerName);

            return lstQuestionModel;
        }
        public async Task UpdateQuestionByIdAync(QuestionModel question)
        {
            await _cosmosDBService.CreateOrUpdateItemAsync(question, question.QuestionId, _cosmosDbName, _cosmosDbContainerName);
        }
    }
}
