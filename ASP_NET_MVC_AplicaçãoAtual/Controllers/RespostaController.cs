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
    public class RespostaController : Controller
    {
        private readonly WebAppContext _context;

        public RespostaController(WebAppContext context)
        {
            _context = context;
        }

        public IActionResult Negado()
        {
            return View();
        }

        // GET: Resposta
        public async Task<IActionResult> Index()
        {
            return View(await _context.Resposta.FromSqlRaw("SELECT * FROM Resposta ORDER BY DataCriacao DESC;").ToListAsync());
        }

        // GET: Resposta/Details/5
        public async Task<IActionResult> Details(string id)
        {
            //if (Program.Admin == null)
            //{
            //    return RedirectToAction("Negado");
            //}

            if (id == null || _context.Resposta == null)
            {
                return NotFound();
            }

            string sql = $"SELECT * FROM Resposta WHERE Protocolo = '{id}'";

            var resposta = await _context.Resposta.FromSqlRaw(sql).ToListAsync();

            string sql2 = $"SELECT * FROM Manifestacao WHERE Protocolo = {id}";

            var m = await _context.Manifestacao.FromSqlRaw(sql2).ToListAsync();

            if (resposta == null)
            {
                return NotFound();
            }

            Resposta r = resposta.FirstOrDefault();
            r.Manifestacao = m.FirstOrDefault();

            return View(r);

        }

        public IActionResult Create()
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            return View();
        }

        // GET: Resposta/Create
        //public IActionResult Create()
        //{
        //    if (Program.Admin == null)
        //    {
        //        return RedirectToAction("Negado");
        //    }

        //    ViewData["Protocolo"] = new SelectList(_context.Manifestacao, "Protocolo", "Protocolo");
        //    return View();
        //}

        // POST: Resposta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Protocolo,Texto,DataCriacao")] Resposta resposta)
        {

            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (resposta != null)
            {
                string sql = $"INSERT INTO Resposta ([DataCriacao], [ID], [Protocolo], [Texto]) VALUES ('{DateTime.Now}', '000', '{ManifestacaoController.GenerateRandomString(6)}', '{resposta.Texto}')";

                await _context.Database.ExecuteSqlRawAsync(sql);

                return RedirectToAction(nameof(Index));
            }

            return View(resposta);
        }

        // GET: Resposta/Edit/5
        public async Task<IActionResult> Edit(string id)
        {

            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (id == null || _context.Resposta == null)
            {
                return NotFound();
            }

            string sql = $"SELECT * FROM Resposta WHERE Protocolo = {id}";

            var resposta = await _context.Resposta.FromSqlRaw(sql).ToListAsync();

            if (resposta == null)
            {
                return NotFound();
            }

            return View(resposta.FirstOrDefault());
        }

        // POST: Resposta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Protocolo,Texto,DataCriacao")] Resposta resposta)
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (id != resposta.Protocolo)
            {
                return NotFound();
            }

            if (resposta != null)
            {
                try
                {
                    string sql = $"UPDATE Resposta SET Texto = '{resposta.Texto}' WHERE Protocolo = '{id}';";
                    await _context.Database.ExecuteSqlRawAsync(sql);

                    string sql2 = $"UPDATE Manifestacao SET DataResposta = '{DateTime.Now}' WHERE Protocolo = '{id}';";
                    await _context.Database.ExecuteSqlRawAsync(sql2);
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
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

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
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

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
