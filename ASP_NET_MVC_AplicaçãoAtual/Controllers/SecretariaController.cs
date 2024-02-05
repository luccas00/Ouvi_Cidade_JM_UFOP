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
    public class SecretariaController : Controller
    {

        private readonly WebAppContext _context;


        public SecretariaController(WebAppContext context)
        {
            _context = context;
        }
        public IActionResult Negado()
        {
            return View();
        }
        // GET: Secretaria
        public async Task<IActionResult> Index()
        {
              return _context.Secretaria != null ? 
                          View(await _context.Secretaria.FromSqlRaw("SELECT * FROM Secretaria").ToListAsync()) :
                          Problem("Entity set 'WebAppContext.Secretaria'  is null.");
        }

        // GET: Secretaria/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (id == null || _context.Secretaria == null)
            {
                return NotFound();
            }

            string sql = $"SELECT * FROM Secretaria WHERE Id = '{id}'";

            var secretaria = await _context.Secretaria.FromSqlRaw(sql).ToListAsync();

            if (secretaria == null)
            {
                return NotFound();
            }

            return View(secretaria.FirstOrDefault());
        }

        // GET: Secretaria/Create
        public IActionResult Create()
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            return View();
        }

        // POST: Secretaria/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Secretaria secretaria)
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (secretaria != null)
            {
                string sql = $"INSERT INTO Secretaria ([Id], [Nome]) VALUES ('{secretaria.Id}', '{secretaria.Nome}')";
                    
                await _context.Database.ExecuteSqlRawAsync(sql);

                return RedirectToAction(nameof(Index));
            }
            return View(secretaria);
        }

        // GET: Secretaria/Edit/5
        public async Task<IActionResult> Edit(string id)
        {

            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (id == null || _context.Secretaria == null)
            {
                return NotFound();
            }

            string sql = $"SELECT * FROM Secretaria WHERE Id = {id}";

            var secretaria = await _context.Secretaria.FromSqlRaw(sql).ToListAsync();

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

            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (id != secretaria.Id)
            {
                return NotFound();
            }

            if (secretaria != null)
            {
                try
                {
                    string sql = $"UPDATE Secretaria SET Nome = '{secretaria.Nome}' WHERE Id = '{id}';";
                    await _context.Database.ExecuteSqlRawAsync(sql);
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

            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }


            if (id == null || _context.Secretaria == null)
            {
                return NotFound();
            }

            string sql = $"SELECT * FROM Secretaria WHERE Id = '{id}'";

            var secretaria = await _context.Secretaria.FromSqlRaw(sql).ToArrayAsync();

            if (secretaria == null)
            {
                return NotFound();
            }

            return View(secretaria.FirstOrDefault());
        }

        // POST: Secretaria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (_context.Secretaria == null)
            {
                return Problem("Entity set 'WebAppContext.Secretaria'  is null.");
            }

            string sql = $"SELECT * FROM Secretaria WHERE Id = '{id}'";

            var secretaria = await _context.Secretaria.FromSqlRaw(sql).ToArrayAsync();

            if (secretaria != null)
            {
                string sql2 = $"DELETE FROM Secretaria WHERE Id = '{id}'";
                _context.Database.ExecuteSqlRaw(sql2);
            }

            return RedirectToAction(nameof(Index));

        }

        private bool SecretariaExists(string id)
        {
            string sql = $"SELECT * FROM Secretaria WHERE Id = '{id}'";
            var aux = _context.Secretaria.FromSqlRaw(sql).ToList();
            return aux.Count > 0 ? true : false;
        }
    }
}
