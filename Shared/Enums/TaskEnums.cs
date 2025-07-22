using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace JoinBackendDotnet.Models
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum Priority
    {
        [EnumMember(Value = "Low")]
        Low,

        [EnumMember(Value = "Medium")]
        Medium,

        [EnumMember(Value = "Urgent")]
        Urgent
    }

    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum Category
    {
        [EnumMember(Value = "Bug")]
        Bug,

        [EnumMember(Value = "User Story")]
        UserStory,

        [EnumMember(Value = "Technical Task")]
        TechnicalTask
    }

    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum Status
    {
        [EnumMember(Value = "To do")]
        ToDo,

        [EnumMember(Value = "In progress")]
        InProgress,

        [EnumMember(Value = "Await feedback")]
        AwaitFeedback,

        [EnumMember(Value = "Done")]
        Done
    }
}
