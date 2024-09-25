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
        public async Task<IActionResult> Create([Bind("AuthorId,Title,Date,Description,LocationId,Id")] Photo photo, IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("Image", "Будь ласка, оберіть зображення.");
            }

            if (ModelState.IsValid)
            {
                photo.Image = await ConvertImageToByteArrayAsync(imageFile);

                // Перевірка на наявність фотографії
                if (!await IsPhotoExists(photo.Title, photo.Image, photo.AuthorId, photo.Id))
                {
                    _context.Add(photo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Image", "У цього автора вже додано це зображення з ідентичним підписом. Будь ласка, оберіть інше зображення або придумаєте інший підпис.");
                }
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
        public async Task<IActionResult> Edit(int id, [Bind("AuthorId,Title,Date,Description,LocationId,Image,Id")] Photo photo, IFormFile imageFile)
        {
            if (id != photo.Id)
            {
                return NotFound();
            }

            var existingPhoto = await _context.Photos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            if (existingPhoto == null)
                return NotFound();


            if (imageFile == null || imageFile.Length == 0)
                photo.Image = existingPhoto.Image;


            if (imageFile != null && imageFile.Length > 0)
                photo.Image = await ConvertImageToByteArrayAsync(imageFile);


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
                ModelState.AddModelError("Image", "У цього автора вже додано це зображення з ідентичним підписом. Будь ласка, оберіть інше зображення або придумаєте інший підпис.");


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
            if (photo == null)
                return Json(new { success = false, message = "Фото не знайдено." });


            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Фото успішно видалено." });
        }

        private async Task<byte[]> ConvertImageToByteArrayAsync(IFormFile imageFile)
        {
            using (var ms = new MemoryStream())
            {
                await imageFile.CopyToAsync(ms);
                return ms.ToArray();
            }
        }

        private bool PhotoExists(int id)
        {
            return _context.Photos.Any(e => e.Id == id);
        }
        private async Task<bool> IsPhotoExists(string title, byte[]? image, int authorId, int id)
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
