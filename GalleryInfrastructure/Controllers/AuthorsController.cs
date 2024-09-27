using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GalleryDomain.Model;
using GalleryInfrastructure;
using Humanizer.Localisation;

namespace GalleryInfrastructure.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly GalleryContext _context;

        public AuthorsController(GalleryContext context)
        {
            _context = context;
        }

        // GET: Authors
        public async Task<IActionResult> Index()
        {
            var galleryContext = _context.Authors.Include(a => a.Country);
            return View(await galleryContext.ToListAsync());
        }
        public async Task<IActionResult> AuthorsByCountry(int? id, string? name)
        {
            if(id == null)
                return RedirectToAction("Countries", "Index");

            ViewBag.CountryId = id;
            ViewBag.CountryName = name;

            var authorsBycountry = _context.Authors.Where(a => a.CountryId == id).Include(a => a.Country);
            return View(await authorsBycountry.ToListAsync());
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .Include(a => a.Country)
                .Include(a => a.Photos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,CountryId,Biography,Id")] Author author)
        {
            if (ModelState.IsValid)
            {
                if (!await IsAuthorExists(author.Name, author.Biography, author.Id))
                {
                    _context.Add(author);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                    ModelState.AddModelError("Biography", "Автор з таким іменем і біографією вже існує.");
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", author.CountryId);
            return View(author);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", author.CountryId);
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,CountryId,Biography,Id")] Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!await IsAuthorExists(author.Name, author.Biography, author.Id))
                {
                    try
                    {
                        _context.Update(author);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AuthorExists(author.Id))
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
                    ModelState.AddModelError("Biography", "Автор з таким іменем і біографією вже існує.");

            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", author.CountryId);
            return View(author);
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .Include(a => a.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, bool confirmDeletePhotos = false)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
                return NotFound();

            if (await IsAuthorLinkedToPhotos(id))
            {
                if (!confirmDeletePhotos)
                    return Json(new { success = false, message = "До автора прив'язані фото. Підтвердіть видалення автора разом з фотографіями.", confirmRequired = true });


                _context.Photos.RemoveRange(_context.Photos.Where(p => p.AuthorId == id));
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Автор та його фотографії успішно видалені." });
        }
        private async Task<bool> IsAuthorLinkedToPhotos(int authorId)
        {
            return await _context.Photos.AnyAsync(a => a.AuthorId == authorId);
        }
        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
        private async Task<bool> IsAuthorExists(string name, string? biography, int id)
        {
            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.Name == name
                                       && m.Biography == biography
                                       && m.Id != id);

            return author != null;
        }
    }
}
