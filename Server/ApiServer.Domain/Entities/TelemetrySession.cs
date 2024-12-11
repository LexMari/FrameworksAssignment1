using System.Text.Json.Serialization;
using ApiServer.Domain.Enums;

namespace ApiServer.Domain.Entities;

public class TelemetrySession
{
    public Guid Id { get; private set; }
    
    public TelemetrySessionStatus Status { get; private set; }
    
    /// <summary>
    /// Timestamp for session start
    /// </summary>
    [JsonPropertyName("start")]
    public DateTime StartTimestamp { get; private set; }
    
    /// <summary>
    /// Timestamp for session end/cancellation
    /// </summary>
    [JsonPropertyName("end")]
    public DateTime? EndTimestamp { get; private set; }
    
    /// <summary>
    /// User who started the sessions
    /// </summary>
    [JsonIgnore]
    public int UserId { get; private set; }
    [JsonPropertyName("user")]
    public User User { get; private set; }
    
    /// <summary>
    /// The set for thie session
    /// </summary>
    [JsonIgnore]
    public int FlashcardSetId { get; private set; }
    [JsonPropertyName("set")]
    public FlashcardSet FlashcardSet { get; private set; }
    
    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    public TelemetrySession()
    {
        Id = Guid.NewGuid();
        Status = TelemetrySessionStatus.Incomplete;
        StartTimestamp = DateTime.Now;
    }
    
    /// <summary>
    /// Constructor with required parameters
    /// </summary>
    /// <param name="user"></param>
    /// <param name="set"></param>
    public TelemetrySession(User user, FlashcardSet set) : this()
    {
        UserId = user.Id;
        User = user;
        FlashcardSetId = set.Id;
        FlashcardSet = set;
    }
    
    #endregion
    
    #region Mutator methods
    
    /// <summary>
    /// Complete the session
    /// </summary>
    public void CompleteSession()
    {
        if (Status != TelemetrySessionStatus.Incomplete)
        {
            throw new Exception($"Session has already been set as {Status.ToString().ToLower()}");
        }
        
        EndTimestamp = DateTime.Now;
        Status = TelemetrySessionStatus.Complete;
    }
    
    /// <summary>
    /// Abort the session
    /// </summary>
    public void AbortSession()
    {
        if (Status != TelemetrySessionStatus.Incomplete)
        {
            throw new Exception($"Session has already been set as {Status.ToString().ToLower()}");
        }
        
        EndTimestamp = DateTime.Now;
        Status = TelemetrySessionStatus.Aborted;
    }
    
    #endregion
}