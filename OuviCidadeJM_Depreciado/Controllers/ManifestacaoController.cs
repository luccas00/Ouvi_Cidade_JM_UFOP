using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OuviCidadeJM.Context;
using OuviCidadeJM.Models;

namespace OuviCidadeJM.Controllers
{
    public class ManifestacaoController : Controller
    {
        private readonly WebAppContext _context;

        public ManifestacaoController(WebAppContext context)
        {
            _context = context;
        }

        // GET: Manifestacao
        public async Task<IActionResult> Index()
        {
              return _context.Manifestacao != null ? 
                          View(await _context.Manifestacao.ToListAsync()) :
                          Problem("Entity set 'WebAppContext.Manifestacao'  is null.");
        }

        // GET: Manifestacao/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Manifestacao == null)
            {
                return NotFound();
            }

            var manifestacao = await _context.Manifestacao
                .FirstOrDefaultAsync(m => m.Protocolo == id);
            if (manifestacao == null)
            {
                return NotFound();
            }

            return View(manifestacao);
        }

        // GET: Manifestacao/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manifestacao/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Protocolo,Titulo,Texto,DataCriacao,DataResposta")] Manifestacao manifestacao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(manifestacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(manifestacao);
        }

        // GET: Manifestacao/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Manifestacao == null)
            {
                return NotFound();
            }

            var manifestacao = await _context.Manifestacao.FindAsync(id);
            if (manifestacao == null)
            {
                return NotFound();
            }
            return View(manifestacao);
        }

        // POST: Manifestacao/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Protocolo,Titulo,Texto,DataCriacao,DataResposta")] Manifestacao manifestacao)
        {
            if (id != manifestacao.Protocolo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manifestacao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManifestacaoExists(manifestacao.Protocolo))
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
            return View(manifestacao);
        }

        // GET: Manifestacao/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Manifestacao == null)
            {
                return NotFound();
            }

            var manifestacao = await _context.Manifestacao
                .FirstOrDefaultAsync(m => m.Protocolo == id);
            if (manifestacao == null)
            {
                return NotFound();
            }

            return View(manifestacao);
        }

        // POST: Manifestacao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Manifestacao == null)
            {
                return Problem("Entity set 'WebAppContext.Manifestacao'  is null.");
            }
            var manifestacao = await _context.Manifestacao.FindAsync(id);
            if (manifestacao != null)
            {
                _context.Manifestacao.Remove(manifestacao);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ManifestacaoExists(string id)
        {
          return (_context.Manifestacao?.Any(e => e.Protocolo == id)).GetValueOrDefault();
        }
    }
}
