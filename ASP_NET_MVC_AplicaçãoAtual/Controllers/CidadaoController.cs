using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OuviCidadeV3.Context;
using OuviCidadeV3.Models;

namespace OuviCidadeV3.Controllers
{
    public class CidadaoController : Controller
    {
        private readonly WebAppContext _context;

        public CidadaoController(WebAppContext context)
        {
            _context = context;
        }

        public IActionResult Negado()
        {
            return View();
        }

        //public IActionResult Logado()
        //{
        //    return View();
        //}

        public IActionResult Sair()
        {
            Program.Cidadao = null;
            return View();
        }

        public async Task<IActionResult> Index(string search)
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            string sql = "SELECT * FROM Cidadao";

            if (!string.IsNullOrEmpty(search))
            {
                // Verifica se o parâmetro de pesquisa parece ser um CPF
                bool isCpf = search.All(char.IsDigit);

                if (isCpf)
                {
                    sql += $" WHERE CPF = '{search}'";
                }
                else
                {
                    sql += $" WHERE Nome LIKE '%{search}%'";
                }
            }

            var cidadaos = await _context.Cidadao.FromSqlRaw(sql).ToListAsync();

            return View(cidadaos);
        }

        //public IActionResult Index(string search)
        //{
        //    if (Program.Admin == null)
        //    {
        //        return RedirectToAction("Negado");
        //    }

        //    var query = _context.Cidadao.FromSqlRaw("SELECT * FROM Cidadao").ToList();

        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        string sql = $"SELECT * FROM Cidadao WHERE Nome = {search}";
        //        query = _context.Cidadao.FromSqlRaw(sql).ToList();

        //    }

        //    return View(query);
        //}

        // GET: Cidadao/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (id == null || _context.Cidadao == null)
            {
                return NotFound();
            }

            string sql = $"SELECT * FROM Cidadao WHERE CPF = '{id}'";

            var cidadao = await _context.Cidadao.FromSqlRaw(sql).ToListAsync();

            if (cidadao == null)
            {
                return NotFound();
            }

            return View(cidadao.FirstOrDefault());
        }

        public async Task<IActionResult> Logado(string id)
        {

            if (Program.Cidadao == null)
            {
                return RedirectToAction("Negado");
            }

            if (id == null || _context.Cidadao == null)
            {
                return NotFound();
            }

            string sql = $"SELECT * FROM Cidadao WHERE CPF = '{id}'";

            var cidadao = await _context.Cidadao.FromSqlRaw(sql).ToListAsync();

            if (cidadao == null)
            {
                return NotFound();
            }

            return View(cidadao.FirstOrDefault());
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
            if (cidadao != null)
            {
                cidadao.SecretKey = Guid.NewGuid().ToString();
                cidadao.Senha = EncryptPassword(cidadao.Senha, cidadao.SecretKey);

                string sql = $"INSERT INTO Cidadao ([CPF], [Nome], [Email], [Telefone], [Login], [Senha], [SecretKey], [Endereco], [DataNascimento], [Ativo]) VALUES ('{cidadao.CPF}', '{cidadao.Nome}', '{cidadao.Email}', '{cidadao.Telefone}', '{cidadao.Login}', '{cidadao.Senha}', '{cidadao.SecretKey}', '{cidadao.Endereco}', '{cidadao.DataNascimento}', 1)";

                await _context.Database.ExecuteSqlRawAsync(sql);

                string sql2 = $"SELECT * FROM Cidadao WHERE CPF = '{cidadao.CPF}'";
                var aux = _context.Cidadao.FromSqlRaw(sql2).ToList();

                Program.Cidadao = aux.FirstOrDefault();

                return RedirectToAction("Logado", new { id = Program.Cidadao.CPF });
            }
            return View(cidadao);
        }

        // GET: Cidadao/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (id == null || _context.Cidadao == null)
            {
                return NotFound();
            }

            string sql = $"SELECT * FROM Cidadao WHERE CPF = '{id}'";

            var cidadao = await _context.Cidadao.FromSqlRaw(sql).ToListAsync();

            if (cidadao == null && cidadao.Count > 0)
            {
                return NotFound();
            }

            return View(cidadao.FirstOrDefault());
        }

        // POST: Cidadao/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CPF,Nome,Telefone,Email,Login,Senha,SecretKey,Endereco,DataNascimento,Ativo")] Cidadao cidadao)
        {

            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (id != cidadao.CPF)
            {
                return NotFound();
            }

            if (cidadao != null)
            {
                try
                {
                    string sql = $"UPDATE Cidadao SET Nome = '{cidadao.Nome}', Telefone = '{cidadao.Telefone}', Email = '{cidadao.Email}', Endereco = '{cidadao.Endereco}', DataNascimento = '{cidadao.DataNascimento}' WHERE CPF = '{id}';";
                    await _context.Database.ExecuteSqlRawAsync(sql);
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
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (id == null || _context.Cidadao == null)
            {
                return NotFound();
            }

            string sql = $"SELECT * FROM Cidadao WHERE CPF = '{id}'";

            var cidadao = await _context.Cidadao.FromSqlRaw(sql).ToArrayAsync();

            if (cidadao == null)
            {
                return NotFound();
            }

            return View(cidadao.FirstOrDefault());
        }

        // POST: Cidadao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (_context.Cidadao == null)
            {
                return Problem("Entity set 'WebAppContext.Cidadao'  is null.");
            }

            string sql = $"SELECT * FROM Cidadao WHERE CPF = '{id}'";

            var cidadao = await _context.Cidadao.FromSqlRaw(sql).ToArrayAsync();

            if (cidadao != null)
            {
                string sql2 = $"DELETE FROM Cidadao WHERE CPF = '{id}'";
                _context.Database.ExecuteSqlRaw(sql2);
            }

            return RedirectToAction(nameof(Index));

        }

        private bool CidadaoExists(string id)
        {
            string sql = $"SELECT * FROM Cidadao WHERE CPF = '{id}'";
            var aux = _context.Cidadao.FromSqlRaw(sql).ToList();
            return aux.Count > 0 ? true : false;
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

            string sql = $"SELECT * FROM Cidadao WHERE Login = '{Login}';";

            var usuario = await _context.Cidadao.FromSqlRaw(sql).ToListAsync();

            if (usuario != null && usuario.Count > 0)
            {
                string secret = usuario.FirstOrDefault().SecretKey;

                string senhaCriptografada = usuario.FirstOrDefault().Senha;

                if (usuario.Count > 0 && usuario != null && VerifyPassword(Senha, senhaCriptografada, secret))
                {
                    Program.Cidadao = usuario.FirstOrDefault();
                    return RedirectToAction("Logado", new { id = usuario.FirstOrDefault().CPF });
                }
                else
                {
                    return RedirectToAction("Negado");
                }

            }
            else
            {
                return RedirectToAction("Negado");
            }

        }

        public static string EncryptPassword(string password, string key)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = hmac.ComputeHash(passwordBytes);

                return Convert.ToBase64String(hashBytes);
            }
        }

        public static bool VerifyPassword(string password, string hashedPassword, string key)
        {
            string newHash = EncryptPassword(password, key);
            return newHash == hashedPassword;
        }


    }
}
