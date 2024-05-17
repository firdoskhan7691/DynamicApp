using Newtonsoft.Json;

namespace DynamicApplicationCP.Models
{
    public class ApplicationFormModel : ApplicationFormFields
    {
        [JsonProperty("questions")]
        public List<QuestionModel> Questions { get; set; } = new List<QuestionModel>();
    }

    public class ApplicationFormFields
    {
        [JsonProperty("id")]
        public string ProgramId { get; set; } = string.Empty;

        [JsonProperty("programName")]
        public string ProgramName { get; set; } = string.Empty;

        [JsonProperty("programDesc")]
        public string ProgramDesc { get; set; } = string.Empty;

        [JsonProperty("formFields")]
        public List<FormFields> FormFields { get; set; } = new List<FormFields>();
    }

    public class FormFields
    {
        [JsonProperty("fieldName")]
        public string FieldName { get; set; } = string.Empty;

        [JsonProperty("isMandatory")]
        public bool IsMandatory { get; set; } = false;

        [JsonProperty("isHidden")]
        public bool IsHidden { get; set; } = false;

        [JsonProperty("isInternal")]
        public bool IsInternal { get; set; } = false;
    }
}
