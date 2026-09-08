using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MNight_works.Models;
using MNightWorks.Shared.Models;


namespace MNight_works.Controllers
{
    [ApiController]
    [Route("api/restaurants/{restaurantId}/menuitems")]
    public class MenuItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MenuItemsController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<MenuItem>>> GetAll(int restaurantId)
        {
            // .Where(...) filters the query itself — only rows matching this
            // restaurant ever get pulled from the database, not filtered afterward.
            return await _context.MenuItems
                .Where(item => item.RestaurantId == restaurantId)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItem>> GetById(int restaurantId, int id)
        {
            var item = await _context.MenuItems
                .Where(m => m.RestaurantId == restaurantId)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (item == null) return NotFound();
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<MenuItem>> Create(int restaurantId, MenuItem newItem)
        {
            // Force the item to belong to whichever restaurant is in the URL,
            // regardless of what the request body says — the URL is the
            // source of truth for which restaurant this is.
            newItem.RestaurantId = restaurantId;

            _context.MenuItems.Add(newItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { restaurantId, id = newItem.Id }, newItem);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int restaurantId, int id, MenuItem updateItem)
        {
            if (id != updateItem.Id)
            {
                return BadRequest();
            }

            // Confirm this item actually belongs to the restaurant in the URL
            // BEFORE allowing any change — otherwise a mismatched restaurantId
            // could silently move another restaurant's item.
            var existingItem = await _context.MenuItems
                .FirstOrDefaultAsync(m => m.Id == id && m.RestaurantId == restaurantId);

            if (existingItem == null) return NotFound();

            // Ensure the updated item is associated with the restaurant in the URL
            updateItem.RestaurantId = restaurantId;

            // Copy the new values onto the tracked entity rather than attaching a detached one
            _context.Entry(existingItem).CurrentValues.SetValues(updateItem);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        //DELETE: api/restaurants/{restaurantId}/menuitems/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int restaurantId, int id)
        {
            var item = await _context.MenuItems
                .Where(m => m.RestaurantId == restaurantId)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (item == null) return NotFound();

            _context.MenuItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
