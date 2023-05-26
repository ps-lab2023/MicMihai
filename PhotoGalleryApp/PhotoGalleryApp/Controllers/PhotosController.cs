using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhotoGalleryApp.Data;
using PhotoGalleryApp.Models;

namespace PhotoGalleryApp.Controllers
{
    public class PhotosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhotosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string searchType)
        {
            IQueryable<Photo> photos = _context.Photos.Include(p => p.Album);

            if (!string.IsNullOrEmpty(searchString))
            {
                if (searchType == "Tag")
                {
                    photos = photos.Where(p => p.TagPhotos.Any(tp => tp.Tag.Name.Contains(searchString)));
                }
                else // default to title search if searchType is not recognized
                {
                    photos = photos.Where(p => p.Title.ToLower().Contains(searchString.ToLower()));
                }
            }

            return View(await photos.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)//
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
    .Include(p => p.Album)
    .Include(p => p.Comments).ThenInclude(c => c.User)
    .Include(p => p.TagPhotos).ThenInclude(tp => tp.Tag)
    .FirstOrDefaultAsync(m => m.PhotoId == id);

            if (photo == null)
            {
                return NotFound();
            }

            var viewModel = new PhotoDetailViewModels
            {
                Photo = photo,
                NewComment = new Comment(),
                Albums = await _context.Albums.ToListAsync()
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            ViewData["AlbumId"] = new SelectList(_context.Albums, "AlbumId", "Name");
            return View();
        }

        // POST: Photos/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("PhotoId,Title,Url,AlbumId")] Photo photo)
        {
            Console.WriteLine("Entering Create action");
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(photo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AlbumId"] = new SelectList(_context.Albums, "AlbumId", "Name", photo.AlbumId);
            return View(photo);
        }
        // GET: Photos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }

            ViewBag.AlbumId = new SelectList(_context.Albums, "AlbumId", "Name", photo.AlbumId); // Populate the Albums SelectList
            return View(photo);
        }

        // POST: Photos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PhotoId,Title,Url,AlbumId")] Photo photo)
        {
            if (id != photo.PhotoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(photo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotoExists(photo.PhotoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AlbumId = new SelectList(_context.Albums, "AlbumId", "Name", photo.AlbumId); // Populate the Albums SelectList if Model validation fails
            return View(photo);
        }
    


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .Include(p => p.Album)
                .FirstOrDefaultAsync(m => m.PhotoId == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photo = await _context.Photos.FindAsync(id);
            if (photo != null)
            {
                _context.Photos.Remove(photo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddTags(int photoId, string newTags)
        {
            var photo = await _context.Photos
                .Include(p => p.TagPhotos)
                .ThenInclude(tp => tp.Tag)
                .FirstOrDefaultAsync(p => p.PhotoId == photoId);

            if (photo == null)
            {
                return NotFound();
            }

            var tagNames = newTags.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var tagName in tagNames)
            {
                var trimmedTagName = tagName.Trim();
                var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == trimmedTagName);
                if (existingTag == null)
                {
                    existingTag = new Tag { Name = trimmedTagName };
                    _context.Tags.Add(existingTag);
                    await _context.SaveChangesAsync();
                }

                if (photo.TagPhotos.All(tp => tp.Tag.Name != trimmedTagName))
                {
                    var tagPhoto = new TagPhoto { TagId = existingTag.TagId, PhotoId = photo.PhotoId };
                    _context.TagPhotos.Add(tagPhoto);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Details), new { id = photoId });
        }

        [HttpPost]
        public async Task<IActionResult> AddToAlbum(int PhotoId, int AlbumId)
        {
            var photo = await _context.Photos.FindAsync(PhotoId);
            if (photo == null)
            {
                return NotFound();
            }

            photo.AlbumId = AlbumId;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = PhotoId });
        }


        private bool PhotoExists(int id)
        {
            return _context.Photos.Any(e => e.PhotoId == id);
        }
    }
}

