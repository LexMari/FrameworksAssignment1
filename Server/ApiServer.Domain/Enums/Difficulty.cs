using System.Text.Json.Serialization;

namespace ApiServer.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<Difficulty>))]
public enum Difficulty
{
    Easy,
    Medium,
    Hard
}