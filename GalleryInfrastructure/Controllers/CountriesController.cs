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
    public class CountriesController : Controller
    {
        private readonly GalleryContext _context;

        public CountriesController(GalleryContext context)
        {
            _context = context;
        }

        // GET: Countries
        public async Task<IActionResult> Index()
        {
            return View(await _context.Countries.ToListAsync());
        }

        // GET: Countries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            //return View(country);
            return RedirectToAction("AuthorsByCountry", "Authors", new { id = country.Id, name = country.Name });
        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id")] Country country)
        {
            if (ModelState.IsValid)
            {
                if (!await IsCountryExists(country.Name, country.Id))
                {
                    _context.Add(country);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                    ModelState.AddModelError("Name", "Країну з такою назвою вже створено.");

            }
            return View(country);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id")] Country country)
        {
            if (id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!await IsCountryExists(country.Name, country.Id))
                {
                    try
                    {
                        _context.Update(country);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CountryExists(country.Id))
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
                    ModelState.AddModelError("Name", "Країну з такою назвою вже створено.");

            }
            return View(country);
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
                return NotFound();

            if (await IsCountryLinkedToAuthor(id))
                return Json(new { success = false, message = "Неможливо видалити країну, оскільки до неї прив'язані автори фотографій. Спочатку видаліть їх." });

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Країну успішно видалено." });
        }
        private async Task<bool> IsCountryLinkedToAuthor(int countryId)
        {
            return await _context.Authors.AnyAsync(a => a.CountryId == countryId);
        }
        private bool CountryExists(int id)
        {
            return _context.Countries.Any(e => e.Id == id);
        }
        private async Task<bool> IsCountryExists(string name, int id)
        {
            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.Name == name
                                       && m.Id != id);

            return country != null;
        }
    }
}
