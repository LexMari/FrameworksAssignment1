namespace ApiServer.Api.Collections.Models;

/// <summary>
/// Collection request
/// </summary>
public class CollectionRequest
{
    /// <summary>
    /// A comment on the collections
    /// </summary>
    public string Comment { get; set; } = string.Empty;

    /// <summary>
    /// A list of flashcard set Ids
    /// </summary>
    public List<int> Sets { get; set; } = new List<int>();
}