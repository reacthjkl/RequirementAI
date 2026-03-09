using System.Text.Json.Serialization;

namespace RequirementAI.Contract.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AuthProvider
{
    Google,
    Apple,
    Twitter,
    Facebook,
    Local
}