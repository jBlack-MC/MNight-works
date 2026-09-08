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
        //PUT: api/restaurants/{restaurantId}/menuitems/{id}
        //"PUT" is the HTTP method conventionally used for "Replace this entire item with new data"
        public async Task<IActionResult> Update(int restaurantId, int id, MenuItem updateItem)
        {
            // Sanity check: the id in the URL should match the id inside the JSON body being sent
            if (id != updateItem.Id)
            {
                return BadRequest();
            }

            // Ensure the item is associated with the restaurant in the URL
            updateItem.RestaurantId = restaurantId;

            // Tell EF Core "treat this object as changed" so it knows to write it on save
            _context.Entry(updateItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Item may have been deleted or moved to another restaurant
                var stillExists = await _context.MenuItems.AnyAsync(m => m.Id == id && m.RestaurantId == restaurantId);
                if (!stillExists) return NotFound();
                throw;
            }

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
