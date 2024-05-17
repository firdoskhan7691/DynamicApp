using DynamicApplicationCP.Models;

namespace DynamicApplicationCP.Interfaces
{
    public interface IProgramService
    {
        Task CreateProgramAsync(ApplicationFormModel programForm);
        Task<ApplicationFormModel> GetProgramByIdAsync(string programFormId);
    }
}
