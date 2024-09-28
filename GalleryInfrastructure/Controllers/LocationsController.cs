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
using Newtonsoft.Json;

namespace GalleryInfrastructure.Controllers;

public class LocationsController : Controller
{
    private readonly GalleryContext _context;

    public LocationsController(GalleryContext context)
    {
        _context = context;
    }

    // GET: Locations
    public async Task<IActionResult> Index()
    {
        var locations = await _context.Locations.ToListAsync();
        return View(locations);
    }

    // GET: Locations/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();


        var location = await _context.Locations
                        .Include(a => a.Photos)
                        .FirstOrDefaultAsync(m => m.Id == id); 
        
        if (location == null)
            return NotFound();


        return View(location);
    }

    // GET: Locations/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Locations/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Link,Latitude,Longitude")] Location location)
    {
        if (ModelState.IsValid)
        {
            if (!await IsLocationExists(location.Name, location.Id))
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
                ModelState.AddModelError("Name", "Локацію з такою назвою вже створено.");
        }
        return View(location);
    }

    // GET: Locations/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var location = await _context.Locations.FindAsync(id);
        if (location == null)
        {
            return NotFound();
        }
        return View(location);
    }

    // POST: Locations/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Link,Latitude,Longitude")] Location location)
    {
        if (id != location.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(location);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(location.Id))
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
        return View(location);
    }

    // GET: Locations/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var location = await _context.Locations.FirstOrDefaultAsync(m => m.Id == id);
        if (location == null)
        {
            return NotFound();
        }

        return View(location);
    }

    // POST: Locations/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var location = await _context.Locations.FindAsync(id);
        if (location == null)
            return NotFound();

        if (await IsLocationLinkedToPhotos(id))
            return Json(new { success = false, message = "Неможливо видалити локацію, оскільки до неї прив'язані фото." });

        _context.Locations.Remove(location);
        await _context.SaveChangesAsync();
        return Json(new { success = true, message = "Локацію успішно видалено." });
    }

    private async Task<bool> IsLocationLinkedToPhotos(int locationId)
    {
        return await _context.Photos.AnyAsync(p => p.LocationId == locationId);
    }

    private bool LocationExists(int id)
    {
        return _context.Locations.Any(e => e.Id == id);
    }

    private async Task<bool> IsLocationExists(string name, int id)
    {
        return await _context.Locations.AnyAsync(m => m.Name == name && m.Id != id);
    }
}
