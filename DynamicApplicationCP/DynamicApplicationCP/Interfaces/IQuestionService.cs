using DynamicApplicationCP.Models;

namespace DynamicApplicationCP.Interfaces
{
    public interface IQuestionService
    {
        Task AddMultipleQuestionsAsync(List<QuestionModel> questionModel);
        Task AddQuestionAsync(QuestionModel questionModel);
        Task<List<QuestionModel>> GetQuestionsByProgramIdAsync(string programId);

        Task UpdateQuestionByIdAync(QuestionModel questionModel);
        Task<string> DeleteQuestionByIdAsync(string questionId);
    }
}
