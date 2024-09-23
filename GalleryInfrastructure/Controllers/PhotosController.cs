using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GalleryDomain.Model;
using GalleryInfrastructure;
using System.Diagnostics.Metrics;

namespace GalleryInfrastructure.Controllers
{
    public class PhotosController : Controller
    {
        private readonly GalleryContext _context;

        public PhotosController(GalleryContext context)
        {
            _context = context;
        }

        // GET: Photos
        public async Task<IActionResult> Index()
        {
            var galleryContext = _context.Photos.Include(p => p.Author).Include(p => p.Location);
            return View(await galleryContext.ToListAsync());
        }

        // GET: Photos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .Include(p => p.Author)
                .Include(p => p.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // GET: Photos/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name");
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name");
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorId,Title,Date,Description,LocationId,Image,Id")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                if (!await IsPhotoExists(photo.Title, photo.Image, photo.AuthorId, photo.Id))
                {
                    _context.Add(photo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                    ModelState.AddModelError("Image", "Це зображення з такою назвою у цього автора вже існує");

            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", photo.AuthorId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", photo.LocationId);
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", photo.AuthorId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", photo.LocationId);
            return View(photo);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuthorId,Title,Date,Description,LocationId,Image,Id")] Photo photo)
        {
            if (id != photo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!await IsPhotoExists(photo.Title, photo.Image, photo.AuthorId, photo.Id))
                {
                    try
                    {
                        _context.Update(photo);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PhotoExists(photo.Id))
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
                else
                    ModelState.AddModelError("Image", "Це зображення з такою назвою у цього автора вже існує");
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", photo.AuthorId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", photo.LocationId);
            return View(photo);
        }

        // GET: Photos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .Include(p => p.Author)
                .Include(p => p.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
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

        private bool PhotoExists(int id)
        {
            return _context.Photos.Any(e => e.Id == id);
        }
        private async Task<bool> IsPhotoExists(string title, byte[] image, int authorId, int id)
        {
            var photo = await _context.Photos
                .FirstOrDefaultAsync(m => m.Title == title
                                       && m.Image == image
                                       && m.AuthorId == authorId
                                       && m.Id != id);

            return photo != null;
        }
    }
}
