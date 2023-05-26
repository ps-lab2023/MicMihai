using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotogGalleryApp.Data;

namespace PhotogGalleryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /// <summary>
    /// API controller for managing tags.
    /// </summary>
    public class TagController : ControllerBase
    {
        private readonly PhotoGalleryDbContext _context; 

        public TagController(PhotoGalleryDbContext context)

        {
            _context = context;
        }

        // GET: api/Tags
        /// <summary>
        /// Retrieves a list of all tags.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTags()
        {
            return await _context.Tags.ToListAsync();
        }

        // GET: api/Tags/5
        /// <summary>
        /// Retrieves a specific tag by ID.
        /// </summary>
        /// <param name="id">The ID of the tag to retrieve.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> GetTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            return tag;
        }

        // PUT: api/Tags/5
        /// <summary>
        /// Updates a specific tag by ID.
        /// </summary>
        /// <param name="id">The ID of the tag to update.</param>
        /// <param name="tag">The updated tag information.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTag(int id, Tag tag)
        {
            // Load the existing Tag object from the database
            var existingTag = await _context.Tags.FindAsync(id);

            if (existingTag == null)
            {
                return NotFound();
            }

            // Update the existing Tag object with the values from the request
            existingTag.Name = tag.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // POST: api/Tags
        /// <summary>
        /// Creates a new tag.
        /// </summary>
        /// <param name="tag">The tag information to create.</param>
        [HttpPost]
        public async Task<ActionResult<Tag>> PostTag(Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTag", new { id = tag.TagId }, tag);
        }

        // DELETE: api/Tags/5
        /// <summary>
        /// Deletes a specific tag by ID.
        /// </summary>
        /// <param name="id">The ID of the tag to delete.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TagExists(int id)
        {
            return _context.Tags.Any(e => e.TagId == id);
        }
    }
}
