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

        // GET: Manifestacaos
        public async Task<IActionResult> Index()
        {
              return _context.Manifestacoes != null ? 
                          View(await _context.Manifestacoes.ToListAsync()) :
                          Problem("Entity set 'WebAppContext.Manifestacoes'  is null.");
        }

        // GET: Manifestacaos/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Manifestacoes == null)
            {
                return NotFound();
            }

            var manifestacao = await _context.Manifestacoes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (manifestacao == null)
            {
                return NotFound();
            }

            return View(manifestacao);
        }

        // GET: Manifestacaos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manifestacaos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Proprietario,Protocolo,Detalhes,Resumo,DataCriacao,ID")] Manifestacao manifestacao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(manifestacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(manifestacao);
        }

        // GET: Manifestacaos/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Manifestacoes == null)
            {
                return NotFound();
            }

            var manifestacao = await _context.Manifestacoes.FindAsync(id);
            if (manifestacao == null)
            {
                return NotFound();
            }
            return View(manifestacao);
        }

        // POST: Manifestacaos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Proprietario,Protocolo,Detalhes,Resumo,DataCriacao,ID")] Manifestacao manifestacao)
        {
            if (id != manifestacao.ID)
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
                    if (!ManifestacaoExists(manifestacao.ID))
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

        // GET: Manifestacaos/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Manifestacoes == null)
            {
                return NotFound();
            }

            var manifestacao = await _context.Manifestacoes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (manifestacao == null)
            {
                return NotFound();
            }

            return View(manifestacao);
        }

        // POST: Manifestacaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Manifestacoes == null)
            {
                return Problem("Entity set 'WebAppContext.Manifestacoes'  is null.");
            }
            var manifestacao = await _context.Manifestacoes.FindAsync(id);
            if (manifestacao != null)
            {
                _context.Manifestacoes.Remove(manifestacao);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ManifestacaoExists(string id)
        {
          return (_context.Manifestacoes?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
