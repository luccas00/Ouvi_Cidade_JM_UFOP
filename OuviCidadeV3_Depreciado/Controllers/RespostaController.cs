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
    public class RespostaController : Controller
    {
        private readonly WebAppContext _context;

        public RespostaController(WebAppContext context)
        {
            _context = context;
        }

        // GET: Resposta
        public async Task<IActionResult> Index()
        {
              return _context.Resposta != null ? 
                          View(await _context.Resposta.ToListAsync()) :
                          Problem("Entity set 'WebAppContext.Resposta'  is null.");
        }

        // GET: Resposta/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Resposta == null)
            {
                return NotFound();
            }

            var resposta = await _context.Resposta
                .FirstOrDefaultAsync(m => m.Protocolo == id);
            if (resposta == null)
            {
                return NotFound();
            }

            return View(resposta);
        }

        // GET: Resposta/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Resposta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Protocolo,Texto,DataCriacao")] Resposta resposta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(resposta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(resposta);
        }

        // GET: Resposta/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Resposta == null)
            {
                return NotFound();
            }

            var resposta = await _context.Resposta.FindAsync(id);
            if (resposta == null)
            {
                return NotFound();
            }
            return View(resposta);
        }

        // POST: Resposta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Protocolo,Texto,DataCriacao")] Resposta resposta)
        {
            if (id != resposta.Protocolo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resposta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RespostaExists(resposta.Protocolo))
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
            return View(resposta);
        }

        // GET: Resposta/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Resposta == null)
            {
                return NotFound();
            }

            var resposta = await _context.Resposta
                .FirstOrDefaultAsync(m => m.Protocolo == id);
            if (resposta == null)
            {
                return NotFound();
            }

            return View(resposta);
        }

        // POST: Resposta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Resposta == null)
            {
                return Problem("Entity set 'WebAppContext.Resposta'  is null.");
            }
            var resposta = await _context.Resposta.FindAsync(id);
            if (resposta != null)
            {
                _context.Resposta.Remove(resposta);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RespostaExists(string id)
        {
          return (_context.Resposta?.Any(e => e.Protocolo == id)).GetValueOrDefault();
        }
    }
}
