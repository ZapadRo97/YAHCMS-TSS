
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YAHCMS.CulturalService.Models;
using YAHCMS.CulturalService.Persistence;

namespace YAHCMS.CulturalService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        IArtistRepository _repository;

        public ArtistsController(IArtistRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<Artist>>> GetAll()
        {
            List<Artist> result = await _repository.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("types")]
        public async Task<ActionResult<ICollection<ArtistType>>> GetAllTypes()
        {
            List<ArtistType> result = await _repository.GetArtistTypesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Artist>> GetOne(long id)
        {
            Artist result = await _repository.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Artist artist)
        {
            Artist a = await _repository.CreateAsync(artist);
            return Created($"{a.ID}", artist);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            Artist artist = await _repository.GetByIdAsync(id);
            if(artist == null)
                return NotFound();

            _repository.Delete(artist);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] Artist artist, long id)
        {
            artist.ID = id;
            var result = await _repository.Update(artist);
            
            return Ok();
        }
    }
}