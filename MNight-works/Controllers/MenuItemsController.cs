using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MNight_works.Models;

namespace MNight_works.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MenuItemsController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItem>> GetById(int id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            if (item == null) return NotFound();
            return item;
        }

        [HttpGet]
        public async Task<ActionResult<List<MenuItem>>> GetAll()
        {
            return await _context.MenuItems.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<MenuItem>> Create(MenuItem newItem)
        {
            _context.MenuItems.Add(newItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newItem.Id }, newItem);

        }

        [HttpPut("{id}")]
        //PUT: api/MenuItems/5
        //"PUT" is the HTTP method conventionally used for "Replace this entire item with new data"
        public async Task <IActionResult> Update(int id, MenuItem updateItem)
        {
            //Sanity check: the id in the URL should match the id inside the JSON body being sent 
            //if someone send mismatched data ids,that a malformed request, not a dabase problem.
            if (id != updateItem.Id)
            {
                //Http 400 - "your request doesnt make sense as written
                return BadRequest();
            }

            //Tell EF core "treat this object as changed" so it knows to write it on save
            _context.Entry(updateItem).State = EntityState.Modified;

            try
            {
                //actually writes the changes to rasturant.db
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //this only happens in a rare edge case: item got deleted by someone/something
                //elsein between you loading it and saving your chnage
                var stillExists = await _context.MenuItems.AnyAsync(m => m.Id == id);
                if (!stillExists) return NotFound(); //it genuinely got gone
                throw; //something else went - let it surface as a real error instead of hiding it
            }

            return NoContent(); //Http 204 - "It worked, and there nothing new hand back to you
        }

        //DELETE: api/MenuItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            //cant delete something that was never there
            if (item == null) return NotFound();

            _context.MenuItems.Remove(item); //mark it for removal
            await _context.SaveChangesAsync(); //actually remove the row from resturant.db

            return NoContent();
        }

    }
}
