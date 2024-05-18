using DynamicApplicationCP.Interfaces;
using DynamicApplicationCP.Models;

namespace DynamicApplicationCP.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IConfiguration _configuration;
        private readonly CosmosDBService _cosmosDBService;
        private readonly string _cosmosDbName;
        private readonly string _cosmosDbContainerName;

        public QuestionService(IConfiguration configuration, CosmosDBService cosmosDBService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _cosmosDBService = cosmosDBService ?? throw new ArgumentNullException(nameof(cosmosDBService));

            _cosmosDbName = _configuration["CosmosDB:DatabaseName"]
                ?? throw new ArgumentNullException("CosmosDB:DatabaseName configuration is missing in appsettings.json");

            _cosmosDbContainerName = _configuration["CosmosDB:QuestionContainerName"]
                ?? throw new ArgumentNullException("CosmosDB:QuestionContainerName configuration is missing in appsettings.json");
        }

        public async Task AddQuestionAsync(QuestionModel questionModel)
        {
            if (questionModel == null)
            {
                throw new ArgumentNullException(nameof(questionModel));
            }

            questionModel.QuestionId = Guid.NewGuid().ToString();
            await _cosmosDBService.CreateOrUpdateItemAsync(questionModel, questionModel.QuestionId, _cosmosDbName, _cosmosDbContainerName);
        }

        public async Task AddMultipleQuestionsAsync(List<QuestionModel> lstQuestionModel)
        {
            if (lstQuestionModel == null)
            {
                throw new ArgumentNullException(nameof(lstQuestionModel));
            }

            List<Task> concurrentTasks = new List<Task>();

            foreach (var question in lstQuestionModel)
            {
                question.QuestionId = Guid.NewGuid().ToString();
                concurrentTasks.Add(_cosmosDBService.CreateOrUpdateItemAsync(question, question.QuestionId, _cosmosDbName, _cosmosDbContainerName));
            }

            await Task.WhenAll(concurrentTasks);
        }

        public async Task<string> DeleteQuestionByIdAsync(string questionId)
        {
            if (string.IsNullOrEmpty(questionId))
            {
                throw new ArgumentException("Question ID cannot be null or empty", nameof(questionId));
            }

            return await _cosmosDBService.DeleteDocumentAsync(questionId, _cosmosDbName, _cosmosDbContainerName);
        }

        public async Task<List<QuestionModel>> GetQuestionsByProgramIdAsync(string programId)
        {
            if (string.IsNullOrEmpty(programId))
            {
                throw new ArgumentException("Program ID cannot be null or empty", nameof(programId));
            }

            string query = $"SELECT * FROM c WHERE c.programId = '{programId}'";
            List<QuestionModel> lstQuestionModel = await _cosmosDBService.GetDocumentsAsync<QuestionModel>(query, _cosmosDbName, _cosmosDbContainerName);

            return lstQuestionModel;
        }

        public async Task UpdateQuestionByIdAync(QuestionModel question)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            await _cosmosDBService.CreateOrUpdateItemAsync(question, question.QuestionId, _cosmosDbName, _cosmosDbContainerName);
        }
    }
}
