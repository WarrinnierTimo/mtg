using Howest.MagicCards.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Howest.MagicCards.MinimalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DecksController : ControllerBase
    {
        private readonly DecksService _decksService;

        public DecksController(DecksService decksService)
        {
            _decksService = decksService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Deck>>> GetAsync()
        {
            var decks = await _decksService.GetAsync();
            return Ok(decks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Deck>> GetAsync(long id)
        {
            var deck = await _decksService.GetAsync(id);
            if (deck == null)
            {
                return NotFound();
            }
            return Ok(deck);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(Deck newDeck)
        {
            await _decksService.CreateAsync(newDeck);
            return CreatedAtAction(nameof(GetAsync), new { id = newDeck.Id }, newDeck);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(long id, Deck updateDeck)
        {
            var deck = await _decksService.GetAsync(id);
            if (deck == null)
            {
                return NotFound();
            }
            await _decksService.UpdateAsync(id, updateDeck);
            return NoContent();
        }
    }
}
