using DynamicApplicationCP.Interfaces;
using DynamicApplicationCP.Models;

namespace DynamicApplicationCP.Services
{
    public class ProgramService : IProgramService
    {
        private readonly IConfiguration _configuration;
        private readonly ICosmosDBService _cosmosDBService;
        private readonly IQuestionService _questionService;
        private readonly string _cosmosDbName;
        private readonly string _cosmosDbContainerName;

        public ProgramService(IConfiguration configuration, ICosmosDBService cosmosDBService, IQuestionService questionService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _cosmosDBService = cosmosDBService ?? throw new ArgumentNullException(nameof(cosmosDBService));
            _questionService = questionService ?? throw new ArgumentNullException(nameof(questionService));

            _cosmosDbName = _configuration["CosmosDB:DatabaseName"]
                ?? throw new ArgumentNullException("CosmosDB:DatabaseName configuration is missing in appsettings.json");

            _cosmosDbContainerName = _configuration["CosmosDB:ContainerName"]
                ?? throw new ArgumentNullException("CosmosDB:ContainerName configuration is missing in appsettings.json");
        }

        public async Task CreateProgramAsync(ApplicationFormModel applicationFormModel)
        {
            if (applicationFormModel == null)
            {
                throw new ArgumentNullException(nameof(applicationFormModel));
            }

            ApplicationFormFields formFields = new ApplicationFormFields
            {
                ProgramId = applicationFormModel.ProgramId,
                ProgramName = applicationFormModel.ProgramName,
                ProgramDesc = applicationFormModel.ProgramDesc,
                FormFields = applicationFormModel.FormFields
            };

            await _cosmosDBService.CreateOrUpdateItemAsync(formFields, formFields.ProgramId, _cosmosDbName, _cosmosDbContainerName);

            // Assign programId to each question
            applicationFormModel.Questions.ForEach(x => x.ProgramId = applicationFormModel.ProgramId);

            await _questionService.AddMultipleQuestionsAsync(applicationFormModel.Questions);
        }

        public async Task<ApplicationFormModel> GetProgramByIdAsync(string programFormId)
        {
            if (string.IsNullOrEmpty(programFormId))
            {
                throw new ArgumentException("Program ID cannot be null or empty", nameof(programFormId));
            }

            string query = $"SELECT * FROM c WHERE c.id = '{programFormId}'";

            List<ApplicationFormFields> formFieldsList = await _cosmosDBService
                .GetDocumentsAsync<ApplicationFormFields>(query, _cosmosDbName, _cosmosDbContainerName);

            if (formFieldsList?.Count > 0)
            {
                var formFields = formFieldsList.FirstOrDefault();

                List<QuestionModel> lstQuestionModel = await _questionService.GetQuestionsByProgramIdAsync(programFormId);

                ApplicationFormModel applicationFormModel = new ApplicationFormModel
                {
                    ProgramId = formFields.ProgramId,
                    ProgramName = formFields.ProgramName,
                    ProgramDesc = formFields.ProgramDesc,
                    FormFields = formFields.FormFields,
                    Questions = lstQuestionModel
                };

                return applicationFormModel;
            }

            return null;
        }
    }
}
