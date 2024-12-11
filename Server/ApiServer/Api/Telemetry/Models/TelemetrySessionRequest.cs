using System.Text.Json.Serialization;

namespace ApiServer.Api.Telemetry.Models;

/// <summary>
/// Request to start a telemetry session for the current user on the passed Flashcard Set ID
/// </summary>
public class TelemetrySessionRequest
{
    /// <summary>
    /// Set Id
    /// </summary>
    public int SetId { get; set; }
}