
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YAHCMS.CulturalService.Models;
using YAHCMS.CulturalService.Persistence;

namespace YAHCMS.CulturalService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        ILocationRepository _repository;
        public LocationsController(ILocationRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<Location>>> GetAll()
        {
            List<Location> result = await _repository.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetOne(long id)
        {
            Location result = await _repository.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Location location)
        {
            Location l = await _repository.CreateAsync(location);
            return Created($"{l.ID}", location);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            Location location = await _repository.GetByIdAsync(id);
            if(location == null)
                return NotFound();

            _repository.Delete(location);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] Location location, long id)
        {
            location.ID = id;
            var  result = await _repository.Update(location);

            return Ok();
        }

    }
}