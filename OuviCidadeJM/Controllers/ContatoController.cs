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
    public class ContatoController : Controller
    {
        private readonly WebAppContext _context;

        public ContatoController(WebAppContext context)
        {
            _context = context;
        }

        // GET: Contatoes
        public async Task<IActionResult> Index()
        {
              return _context.Contatos != null ? 
                          View(await _context.Contatos.ToListAsync()) :
                          Problem("Entity set 'ContatosContext.Contatos'  is null.");
        }

        // GET: Contatoes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Contatos == null)
            {
                return NotFound();
            }

            var contato = await _context.Contatos
                .FirstOrDefaultAsync(m => m.CPF == id);
            if (contato == null)
            {
                return NotFound();
            }

            return View(contato);
        }

        // GET: Contatoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contatoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Telefone,CPF,DataNascimento,Idade,Ativo")] Contato contato)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contato);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contato);
        }

        // GET: Contatoes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Contatos == null)
            {
                return NotFound();
            }

            var contato = await _context.Contatos.FindAsync(id);
            if (contato == null)
            {
                return NotFound();
            }
            return View(contato);
        }

        // POST: Contatoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Nome,Telefone,CPF,DataNascimento,Idade,Ativo")] Contato contato)
        {
            if (id != contato.CPF)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contato);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContatoExists(contato.CPF))
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
            return View(contato);
        }

        // GET: Contatoes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Contatos == null)
            {
                return NotFound();
            }

            var contato = await _context.Contatos
                .FirstOrDefaultAsync(m => m.CPF == id);
            if (contato == null)
            {
                return NotFound();
            }

            return View(contato);
        }

        // POST: Contatoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Contatos == null)
            {
                return Problem("Entity set 'ContatosContext.Contatos'  is null.");
            }
            var contato = await _context.Contatos.FindAsync(id);
            if (contato != null)
            {
                _context.Contatos.Remove(contato);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContatoExists(string id)
        {
          return (_context.Contatos?.Any(e => e.CPF == id)).GetValueOrDefault();
        }
    }
}
