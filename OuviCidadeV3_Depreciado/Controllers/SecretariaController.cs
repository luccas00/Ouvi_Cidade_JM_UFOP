using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OuviCidadeJM.Models;
using OuviCidadeJM_V3.Data;

namespace OuviCidadeJM_V3.Controllers
{
    public class SecretariaController : Controller
    {
        private readonly WebAppContext _context;

        public SecretariaController(WebAppContext context)
        {
            _context = context;
        }

        // GET: Secretaria
        public async Task<IActionResult> Index()
        {
              return _context.Secretaria != null ? 
                          View(await _context.Secretaria.ToListAsync()) :
                          Problem("Entity set 'WebAppContext.Secretaria'  is null.");
        }

        // GET: Secretaria/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Secretaria == null)
            {
                return NotFound();
            }

            var secretaria = await _context.Secretaria
                .FirstOrDefaultAsync(m => m.Id == id);
            if (secretaria == null)
            {
                return NotFound();
            }

            return View(secretaria);
        }

        // GET: Secretaria/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Secretaria/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Secretaria secretaria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(secretaria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(secretaria);
        }

        // GET: Secretaria/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Secretaria == null)
            {
                return NotFound();
            }

            var secretaria = await _context.Secretaria.FindAsync(id);
            if (secretaria == null)
            {
                return NotFound();
            }
            return View(secretaria);
        }

        // POST: Secretaria/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Nome")] Secretaria secretaria)
        {
            if (id != secretaria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(secretaria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SecretariaExists(secretaria.Id))
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
            return View(secretaria);
        }

        // GET: Secretaria/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Secretaria == null)
            {
                return NotFound();
            }

            var secretaria = await _context.Secretaria
                .FirstOrDefaultAsync(m => m.Id == id);
            if (secretaria == null)
            {
                return NotFound();
            }

            return View(secretaria);
        }

        // POST: Secretaria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Secretaria == null)
            {
                return Problem("Entity set 'WebAppContext.Secretaria'  is null.");
            }
            var secretaria = await _context.Secretaria.FindAsync(id);
            if (secretaria != null)
            {
                _context.Secretaria.Remove(secretaria);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SecretariaExists(string id)
        {
          return (_context.Secretaria?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
