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
    public class CidadaoController : Controller
    {
        private readonly WebAppContext _context;

        public CidadaoController(WebAppContext context)
        {
            _context = context;
        }

        // GET: Cidadao
        //public async Task<IActionResult> Index()
        //{
        //    var result = await _context.Cidadao.FromSqlRaw("select * from Cidadao").ToListAsync();

        //    return View(result);
        //}

        public IActionResult Index(string search)
        {
            var query = _context.Cidadao.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Nome.Contains(search));

            }

            var cidadaos = query.ToList();

            return View(cidadaos);
        }

        //public async Task<IActionResult> Search(string search)
        //{
        //    string sql = $"SELECT * FROM Cidadao WHERE Nome = '{search}';";

        //    var usuario = await _context.Cidadao.FromSqlRaw(sql).ToListAsync();

        //    return View(usuario);
        //}


        // GET: Cidadao/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Cidadao == null)
            {
                return NotFound();
            }

            var cidadao = await _context.Cidadao
                .FirstOrDefaultAsync(m => m.CPF == id);
            if (cidadao == null)
            {
                return NotFound();
            }

            return View(cidadao);
        }

        // GET: Cidadao/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cidadao/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CPF,Nome,Telefone,Email,Login,Senha,SecretKey,Endereco,DataNascimento,Ativo")] Cidadao cidadao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cidadao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cidadao);
        }

        // GET: Cidadao/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Cidadao == null)
            {
                return NotFound();
            }

            var cidadao = await _context.Cidadao.FindAsync(id);
            if (cidadao == null)
            {
                return NotFound();
            }
            return View(cidadao);
        }

        // POST: Cidadao/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CPF,Nome,Telefone,Email,Login,Senha,SecretKey,Endereco,DataNascimento,Ativo")] Cidadao cidadao)
        {
            if (id != cidadao.CPF)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cidadao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CidadaoExists(cidadao.CPF))
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
            return View(cidadao);
        }

        // GET: Cidadao/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Cidadao == null)
            {
                return NotFound();
            }

            var cidadao = await _context.Cidadao
                .FirstOrDefaultAsync(m => m.CPF == id);
            if (cidadao == null)
            {
                return NotFound();
            }

            return View(cidadao);
        }

        // POST: Cidadao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Cidadao == null)
            {
                return Problem("Entity set 'WebAppContext.Cidadao'  is null.");
            }
            var cidadao = await _context.Cidadao.FindAsync(id);
            if (cidadao != null)
            {
                _context.Cidadao.Remove(cidadao);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CidadaoExists(string id)
        {
          return (_context.Cidadao?.Any(e => e.CPF == id)).GetValueOrDefault();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Login([Bind("Login,Senha")] Cidadao cidadao)
        {
            string Login = cidadao.Login;
            string Senha = cidadao.Senha;

            string sql = $"SELECT * FROM Cidadao WHERE Login = '{Login}' AND Senha = '{Senha}' ;";

            var usuario = await _context.Cidadao.FromSqlRaw(sql).ToListAsync();

            if (usuario != null)
            {
                Program.Cidadao = usuario.FirstOrDefault();
                ViewBag.Mensagem = "Login Realizado com Sucesso !";
                return RedirectToAction("Details", new { id = usuario.FirstOrDefault().CPF });
            }
            else
            {
                // Exiba uma mensagem de erro se as credenciais estiverem incorretas
                ViewBag.ErrorMessage = "Credenciais inválidas";
                return NotFound();
            }
        }


    }
}
