using DynamicApplicationCP.Interfaces;
using DynamicApplicationCP.Models;

namespace DynamicApplicationCP.Services
{
    public class ProgramService : IProgramService
    {
        private IConfiguration _configuration;
        private CosmosDBService _cosmosDBService;
        private readonly IQuestionService _questionService;
        private string _cosmosDbName;
        private string _cosmosDbContainerName;
        public ProgramService(IConfiguration configuration, CosmosDBService cosmosDBService, IQuestionService questionService)
        {
            _configuration = configuration;
            _cosmosDBService = cosmosDBService;
            _cosmosDbName = _configuration["CosmosDB:DatabaseName"];
            _cosmosDbContainerName = _configuration["CosmosDB:ContainerName"];
            _questionService = questionService;
        }
        public async Task CreateProgramAsync(ApplicationFormModel applicationFormModel)
        {

            ApplicationFormFields formFields = new()
            {
                ProgramId = applicationFormModel.ProgramId,
                ProgramName = applicationFormModel.ProgramName,
                ProgramDesc = applicationFormModel.ProgramDesc,
                FormFields = applicationFormModel.FormFields

            };

            await _cosmosDBService.CreateOrUpdateItemAsync(formFields, formFields.ProgramId, _cosmosDbName, _cosmosDbContainerName);

            applicationFormModel.Questions.ForEach(x => x.ProgramId = applicationFormModel.ProgramId);

            await _questionService.AddMultipleQuestionsAsync(applicationFormModel.Questions);
        }

        public async Task<ApplicationFormModel> GetProgramByIdAsync(string programFormId)
        {
            ApplicationFormModel applicationFormModel = new();

            string query = $"SELECT * FROM c WHERE c.id = '{programFormId}'";

            List<ApplicationFormModel> lsApplicationFormModel = await _cosmosDBService.
                                GetDocumentsAsync<ApplicationFormModel>(query, _cosmosDbName, _cosmosDbContainerName);

            List<QuestionModel> lstQuestionModel = await _questionService.GetQuestionsByProgramIdAsync(programFormId);

            if (lsApplicationFormModel?.Count > 0)
            {
                applicationFormModel = lsApplicationFormModel?.FirstOrDefault();
                applicationFormModel.Questions = lstQuestionModel;
            }

            return applicationFormModel;
        }
    }
}
