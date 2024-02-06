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
    public class ManifestacaoController : Controller
    {
        private readonly WebAppContext _context;

        public ManifestacaoController(WebAppContext context)
        {
            _context = context;
        }

        //public async Task<IActionResult> Index(string search)
        //{

        //    string sql = "SELECT * FROM Manifestacao";

        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        sql += $" WHERE Protocolo = '{search}'";
        //    }

        //    sql += " ORDER BY DataCriacao DESC";

        //    var manifestacoes = await _context.Manifestacao.FromSqlRaw(sql).ToListAsync();

        //    return View(manifestacoes);
        //}

        public async Task<IActionResult> Index(string search, int? secretaria)
        {

            string sql = "SELECT * FROM Manifestacao";

            if (!string.IsNullOrEmpty(search))
            {
                sql += $" WHERE Protocolo = '{search}'";
            }

            if (string.IsNullOrEmpty(search) & secretaria.HasValue)
            {
                sql += $" WHERE SecretariaId = '{secretaria}'";

            } else if (!string.IsNullOrEmpty(search) & secretaria.HasValue)
            {
                sql += $" AND SecretariaId = '{secretaria}'";
            }

            sql += " ORDER BY DataCriacao DESC";

            var manifestacoes = await _context.Manifestacao.FromSqlRaw(sql).ToListAsync();

            ViewBag.Secretarias = await _context.Secretaria.FromSqlRaw("Select * From Secretaria").ToListAsync();

            return View(manifestacoes);
        }


        //// GET: Manifestacao
        //public async Task<IActionResult> Index()
        //{
        //    string sql = $"SELECT * FROM Manifestacao ORDER BY DataCriacao DESC";

        //      return _context.Manifestacao != null ? 
        //                  View(await _context.Manifestacao.FromSqlRaw(sql).ToListAsync()) :
        //                  Problem("Entity set 'WebAppContext.Manifestacao'  is null.");
        //}

        public async Task<IActionResult> Responder()
        {
            string sql = $"SELECT * FROM Manifestacao WHERE DataResposta IS NULL ORDER BY DataCriacao DESC";

            return _context.Manifestacao != null ?
                        View(await _context.Manifestacao.FromSqlRaw(sql).ToListAsync()) :
                        Problem("Entity set 'WebAppContext.Manifestacao'  is null.");
        }

        // GET: Manifestacao/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

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

        public IActionResult Negado()
        {
            return View();
        }

        // POST: Manifestacao/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Titulo,Texto,Secretaria")] Manifestacao manifestacao)
        {
            if (manifestacao != null)
            {
                manifestacao.Protocolo = GenerateRandomString(6);
                manifestacao.DataCriacao = DateTime.Now;
                manifestacao.Secretaria = manifestacao.Secretaria == null ? new Secretaria() : manifestacao.Secretaria;

                if (Program.Cidadao != null)
                {
                    manifestacao.Proprietario = Program.Cidadao;
                   
                } else
                {
                    manifestacao.Proprietario = null;
                }

                string proprietario = manifestacao.Proprietario == null ? "NULL" : "'"+ manifestacao.Proprietario.CPF + "'";

                string sql1 = $"INSERT INTO Resposta ([DataCriacao], [ID], [Protocolo], [Texto]) VALUES ('1990-01-01', '000', '{manifestacao.Protocolo}', '')";

                string sql2 = $"INSERT INTO Manifestacao([ProprietarioCPF], [Titulo], [Texto], [Protocolo], [SecretariaId], [DataCriacao]) VALUES ({proprietario}, '{manifestacao.Titulo}', '{manifestacao.Texto}', '{manifestacao.Protocolo}', '{manifestacao.Secretaria.Id}', '{manifestacao.DataCriacao}')";

                await _context.Database.ExecuteSqlRawAsync(sql1);

                await _context.Database.ExecuteSqlRawAsync(sql2);

                return RedirectToAction(nameof(Index));
            }
            return View(manifestacao);
        }

        public static string GenerateRandomString(int length)
        {
            Random random = new Random();
            const string chars = "0123456789";
            char[] randomArray = new char[length];

            for (int i = 0; i < length; i++)
            {
                randomArray[i] = chars[random.Next(chars.Length)];
            }

            return new string(randomArray);
        }

        // GET: Manifestacao/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

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
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

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
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

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
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

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
