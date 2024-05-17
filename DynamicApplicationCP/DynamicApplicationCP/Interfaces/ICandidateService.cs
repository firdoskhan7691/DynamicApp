using DynamicApplicationCP.Models;

namespace DynamicApplicationCP.Interfaces
{
    public interface ICandidateService
    {
        Task AddCandidateApplication(CandidateModel candidateModel);
    }
}
