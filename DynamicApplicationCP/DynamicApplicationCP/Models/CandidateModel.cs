using Newtonsoft.Json;

namespace DynamicApplicationCP.Models
{
    public class CandidateModel
    {
        [JsonProperty("id")]
        public string CandidateId { get; set; } = string.Empty;

        [JsonProperty("programIdd")]
        public string ProgramId { get; set; } = string.Empty;

        [JsonProperty("firstName")]
        public string FirstName { get; set; } = string.Empty;

        [JsonProperty("lastName")]
        public string LastName { get; set; } = string.Empty;

        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;

        [JsonProperty("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonProperty("nationality")]
        public string Nationalty { get; set; } = string.Empty;

        [JsonProperty("currentResidence")]
        public string CurrentResidence { get; set; } = string.Empty;

        [JsonProperty("idNumber")]
        public string IdNumber { get; set; } = string.Empty;

        [JsonProperty("dateOfBirth")]
        public string DateOfBirth { get; set; } = string.Empty;

        [JsonProperty("gender")]
        public string Gender { get; set; } = string.Empty;

        [JsonProperty("candidateAnswers")]
        public List<QuestionAnswer> CandidateAnswers { get; set; } = new List<QuestionAnswer>();
    }

    public class QuestionAnswer
    {
        [JsonProperty("questionId")]
        public string QuestionId { get; set; } = string.Empty;

        [JsonProperty("questionType")]
        public string QuestionType { get; set; } = string.Empty;

        [JsonProperty("answer")] // Here will add multiple choice ans in comma seprated string
        public string Ansewer { get; set; } = string.Empty;
    }
}
