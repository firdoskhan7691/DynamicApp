using Newtonsoft.Json;

namespace DynamicApplicationCP.Models
{
    public class QuestionModel
    {
        [JsonProperty("id")]
        public string QuestionId { get; set; } = string.Empty;

        [JsonProperty("programId")]
        public string ProgramId { get; set; } = string.Empty;

        [JsonProperty("questionTypeId")]
        public string QuestionTypeId { get; set; } = string.Empty;

        [JsonProperty("questionType")]
        public string QuestionType { get; set; } = string.Empty;

        [JsonProperty("question")]
        public string Question { get; set; } = string.Empty;

        [JsonProperty("choices")]
        public string[] Choices { get; set; }
    }
}
