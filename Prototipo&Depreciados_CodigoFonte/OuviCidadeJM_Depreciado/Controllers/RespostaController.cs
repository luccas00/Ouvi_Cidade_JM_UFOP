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
            var webAppContext = _context.Resposta.Include(r => r.Manifestacao);
            return View(await webAppContext.ToListAsync());
        }

        // GET: Resposta/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Resposta == null)
            {
                return NotFound();
            }

            var resposta = await _context.Resposta
                .Include(r => r.Manifestacao)
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
            ViewData["Protocolo"] = new SelectList(_context.Manifestacao, "Protocolo", "Protocolo");
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
            ViewData["Protocolo"] = new SelectList(_context.Manifestacao, "Protocolo", "Protocolo", resposta.Protocolo);
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
            ViewData["Protocolo"] = new SelectList(_context.Manifestacao, "Protocolo", "Protocolo", resposta.Protocolo);
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
            ViewData["Protocolo"] = new SelectList(_context.Manifestacao, "Protocolo", "Protocolo", resposta.Protocolo);
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
                .Include(r => r.Manifestacao)
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
