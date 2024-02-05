using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OuviCidadeV3.Context;
using OuviCidadeV3.Models;

namespace OuviCidadeV3.Controllers
{
    public class NoticiaController : Controller
    {
        private readonly WebAppContext _context;

        public NoticiaController(WebAppContext context)
        {
            _context = context;
        }

        // GET: Noticia
        public async Task<IActionResult> Index()
        {
              return _context.Noticia != null ? 
                          View(await _context.Noticia.ToListAsync()) :
                          Problem("Entity set 'WebAppContext.Noticia'  is null.");
        }

        // GET: Noticia/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (id == null || _context.Noticia == null)
            {
                return NotFound();
            }

            var noticia = await _context.Noticia
                .FirstOrDefaultAsync(m => m.ID == id);
            if (noticia == null)
            {
                return NotFound();
            }

            return View(noticia);
        }

        // GET: Noticia/Create
        public IActionResult Create()
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            return View();
        }

        // POST: Noticia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Titulo,Texto,DataCriacao,ID")] Noticia noticia)
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }
            if (ModelState.IsValid)
            {
                _context.Add(noticia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(noticia);
        }

        // GET: Noticia/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (id == null || _context.Noticia == null)
            {
                return NotFound();
            }

            var noticia = await _context.Noticia.FindAsync(id);
            if (noticia == null)
            {
                return NotFound();
            }
            return View(noticia);
        }

        // POST: Noticia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Titulo,Texto,DataCriacao,ID")] Noticia noticia)
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (id != noticia.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(noticia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoticiaExists(noticia.ID))
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
            return View(noticia);
        }

        // GET: Noticia/Delete/5
        public async Task<IActionResult> Delete(string id)
        {

            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (id == null || _context.Noticia == null)
            {
                return NotFound();
            }

            var noticia = await _context.Noticia
                .FirstOrDefaultAsync(m => m.ID == id);
            if (noticia == null)
            {
                return NotFound();
            }

            return View(noticia);
        }

        // POST: Noticia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (_context.Noticia == null)
            {
                return Problem("Entity set 'WebAppContext.Noticia'  is null.");
            }
            var noticia = await _context.Noticia.FindAsync(id);
            if (noticia != null)
            {
                _context.Noticia.Remove(noticia);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoticiaExists(string id)
        {
          return (_context.Noticia?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
