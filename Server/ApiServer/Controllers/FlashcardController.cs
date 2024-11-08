using ApiServer.Domain.Entities;
using ApiServer.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ApiServer.Controllers;

[Route("flashcards")]
[ApiController]
public class FlashcardController : Controller
{
    private ILogger<FlashcardController> _logger;
    private readonly ApiContext _context;
    
    #region Constructor
    
    /// <summary>
    /// Default Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="dbContext"></param>
    public FlashcardController(ILogger<FlashcardController> logger, ApiContext dbContext)
    {
        _logger = logger;
        _context = dbContext;
    }
    #endregion
    
    /// <summary>
    /// Get all Flashcards
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFlashcards()
    {
        var responseData = await _context.Flashcards
            .Select(x=> new FlashcardDto(x.Id, x.Question, x.Answer, x.FlashcardSetId))
            .ToListAsync();
        
        return Ok(JsonConvert.SerializeObject(responseData));
    }

    /// <summary>
    /// Create a flashcard
    /// </summary>
    /// <param name="createFlashcard"></param>
    /// <returns></returns>
    public async Task<IActionResult> CreateFlashcard([FromBody] CreateFlashcardDto createFlashcard)
    {
        var flashcard = new Flashcard(
            createFlashcard.Id,
            createFlashcard.Question,
            createFlashcard.Answer,
            createFlashcard.Difficulty,
            createFlashcard.FlashcardSetId);
        try
        {
            await _context.Flashcards.AddAsync(flashcard);
            await _context.SaveChangesAsync();
            var responseData = new FlashcardDto(flashcard.Id, flashcard.Question, flashcard.Answer,
                flashcard.FlashcardSetId);
            return Created(nameof(GetFlashcards), JsonConvert.SerializeObject(responseData));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to create flashcard");
            return BadRequest("Unable to create flashcard record");
        }
    }
    
    #region DTO

    private record FlashcardDto(int Id, string Question, string Answer, int FlashcardSetId);

    public record CreateFlashcardDto(int Id, string Question, string Answer, string Difficulty, int FlashcardSetId);

    #endregion
}