using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MNight_works.Models;
using MNightWorks.Shared;
using MNightWorks.Shared.Models;

namespace MNight_works.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RestaurantsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Restaurant>>> GetAll()
        {
            return await _context.Restaurants.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Restaurant>> GetById(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null) return NotFound();
            return restaurant;
        }

        [HttpPost]
        public async Task<ActionResult<Restaurant>> Create(Restaurant newRestaurant)
        {
            _context.Restaurants.Add(newRestaurant);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newRestaurant.Id }, newRestaurant);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null) return NotFound();

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}