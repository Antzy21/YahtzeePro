using System.Text.Json.Serialization;

namespace YahtzeePro.Core.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MoveChoice
{
    Risky,
    Safe,
}