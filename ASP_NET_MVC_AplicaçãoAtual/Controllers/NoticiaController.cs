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
        public IActionResult Negado()
        {
            return View();
        }

        // GET: Noticia
        public async Task<IActionResult> Index()
        {
              return _context.Noticia != null ? 
                          View(await _context.Noticia.FromSqlRaw("SELECT * FROM Noticia ORDER BY DataCriacao DESC").ToListAsync()) :
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

            string sql = $"SELECT * FROM Noticia WHERE ID = '{id}'";

            var noticia = await _context.Noticia.FromSqlRaw(sql).ToListAsync();
            if (noticia == null)
            {
                return NotFound();
            }

            return View(noticia.FirstOrDefault());
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
        public async Task<IActionResult> Create([Bind("Titulo,Texto")] Noticia noticia)
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (noticia != null)
            {
                string secretaria = Program.Admin.Secretaria == null ? "NULL" : "'" + Program.Admin.Secretaria.Id + "'";
                string sql = $"INSERT INTO Noticia ([ID], [Titulo], [Texto], [ProprietarioID], [SecretariaId], [DataCriacao]) VALUES ('{ManifestacaoController.GenerateRandomString(4)}', '{noticia.Titulo}', '{noticia.Texto}', '{Program.Admin.ID}', {secretaria}, '{DateTime.Now}')";

                await _context.Database.ExecuteSqlRawAsync(sql);

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

            string sql = $"SELECT * FROM Noticia WHERE ID = {id}";


            var noticia = await _context.Noticia.FromSqlRaw(sql).ToListAsync();

            if (noticia == null)
            {
                return NotFound();
            }
            return View(noticia.FirstOrDefault());
        }

        // POST: Noticia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Titulo,Texto")] Noticia noticia)
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (id != noticia.ID)
            {
                return NotFound();
            }

            if (noticia != null)
            {
                try
                {
                    string sql = $"UPDATE Noticia SET Titulo = '{noticia.Titulo}', Texto = '{noticia.Texto}' WHERE ID = '{id}';";
                    await _context.Database.ExecuteSqlRawAsync(sql);
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

            string sql = $"SELECT * FROM Noticia WHERE ID = '{id}'";

            var noticia = await _context.Noticia.FromSqlRaw(sql).ToArrayAsync();

            if (noticia == null)
            {
                return NotFound();
            }

            return View(noticia.FirstOrDefault());
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

            string sql = $"SELECT * FROM Noticia WHERE ID = '{id}'";

            var noticia = await _context.Noticia.FromSqlRaw(sql).ToArrayAsync();

            if (noticia != null)
            {
                string sql2 = $"DELETE FROM Noticia WHERE ID = '{id}'";
                _context.Database.ExecuteSqlRaw(sql2);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool NoticiaExists(string id)
        {
            string sql = $"SELECT * FROM Noticia WHERE ID = '{id}'";
            var aux = _context.Noticia.FromSqlRaw(sql).ToList();
            return aux.Count > 0 ? true : false;
        }
    }
}
