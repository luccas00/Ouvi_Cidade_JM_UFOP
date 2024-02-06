using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
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
    public class AdminController : Controller
    {
        private readonly WebAppContext _context;

        public AdminController(WebAppContext context)
        {
            _context = context;
        }

        public IActionResult Negado()
        {
            return View();
        }

        public IActionResult Sair()
        {
            Program.Admin = null;
            return View();
        }


        // GET: Admin
        public async Task<IActionResult> Index()
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }
                
            return _context.Admin != null ? 
                          View(await _context.Admin.FromSqlRaw("SELECT * FROM Admin").ToListAsync()) :
                          Problem("Entity set 'WebAppContext.Admin'  is null.");
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (id == null || _context.Admin == null)
            {
                return NotFound();
            }

            string sql = $"SELECT * FROM Admin WHERE ID = '{id}'";

            var admin = await _context.Admin.FromSqlRaw(sql).ToListAsync();

            if (admin == null)
            {
                return NotFound();
            }

            return View(admin.FirstOrDefault());
        }

        public async Task<IActionResult> Logado(string id)
        {

            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (id == null || _context.Admin == null)
            {
                return NotFound();
            }

            string sql = $"SELECT * FROM Admin WHERE ID = '{id}'";

            var admin = await _context.Admin.FromSqlRaw(sql).ToListAsync();

            if (admin == null)
            {
                return NotFound();
            }

            return View(admin.FirstOrDefault());
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("negado");
            }

            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nome,Telefone,Email,Login,Senha,Endereco,DataNascimento,Ativo")] Admin admin)
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (admin != null)
            {
                admin.SecretKey = Guid.NewGuid().ToString();
                admin.Senha = EncryptPassword(admin.Senha, admin.SecretKey);

                string sql = $"INSERT INTO Admin ([ID], [Nome], [Email], [Telefone], [Login], [Senha], [SecretKey], [Endereco], [DataNascimento], [SecretariaId], [Ativo]) VALUES ('{admin.ID}', '{admin.Nome}', '{admin.Email}', '{admin.Telefone}', '{admin.Login}', '{admin.Senha}', '{admin.SecretKey}', 'SYSTEM', '{admin.DataNascimento}', NULL, 1)";

                await _context.Database.ExecuteSqlRawAsync(sql);

                string sql2 = $"SELECT * FROM Admin WHERE ID = '{admin.ID}'";
                var aux = _context.Admin.FromSqlRaw(sql2).ToList();

                Program.Admin = aux.FirstOrDefault();

                return RedirectToAction("Logado", new { id = Program.Admin.ID });

            }
            return View(admin);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (id == null || _context.Admin == null)
            {
                return NotFound();
            }

            string sql = $"SELECT * FROM Admin WHERE ID = {id}";

            var admin = await _context.Admin.FromSqlRaw(sql).ToListAsync();

            if (admin == null && admin.Count > 0)
            {
                return NotFound();
            }

            return View(admin.FirstOrDefault());
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Nome,Telefone,Email,Login,Senha,SecretKey,Endereco,DataNascimento,Ativo")] Admin admin)
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (id != admin.ID)
            {
                return NotFound();
            }

            if (admin != null)
            {
                try
                {
                    string sql = $"UPDATE Admin SET Nome = '{admin.Nome}', Telefone = '{admin.Telefone}', Email = '{admin.Email}', Endereco = '{admin.Endereco}', DataNascimento = '{admin.DataNascimento}', SecretariaId = '{admin.Secretaria}', WHERE ID = '{id}';";
                    await _context.Database.ExecuteSqlRawAsync(sql);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(admin.ID))
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
            return View(admin);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (id == null || _context.Admin == null)
            {
                return NotFound();
            }

            string sql = $"SELECT * FROM Admin WHERE ID = '{id}'";

            var admin = await _context.Admin.FromSqlRaw(sql).ToArrayAsync();

            if (admin == null)
            {
                return NotFound();
            }

            return View(admin.FirstOrDefault());
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (Program.Admin == null)
            {
                return RedirectToAction("Negado");
            }

            if (_context.Admin == null)
            {
                return Problem("Entity set 'WebAppContext.Admin'  is null.");
            }

            string sql = $"SELECT * FROM Admin WHERE ID = '{id}'";

            var admin = await _context.Admin.FromSqlRaw(sql).ToArrayAsync();

            if (admin != null)
            {
                string sql2 = $"DELETE FROM Admin WHERE ID = '{id}'";
                _context.Database.ExecuteSqlRaw(sql2);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AdminExists(string id)
        {
            string sql = $"SELECT * FROM Admin WHERE ID = '{id}'";
            var aux = _context.Admin.FromSqlRaw(sql).ToList();
            return aux.Count > 0 ? true : false;
        }

        public ActionResult AdmLogin()
        {
            return View();
        }

        [HttpPost, ActionName("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdmLogin([Bind("Login,Senha")] Admin admin)
        {
            string Login = admin.Login;
            string Senha = admin.Senha;

            string sql = $"SELECT * FROM Admin WHERE Login = '{Login}';";

            var usuario = await _context.Admin.FromSqlRaw(sql).ToListAsync();

            if(usuario != null && usuario.Count > 0)
            {
                string secret = usuario.FirstOrDefault().SecretKey;

                string senhaCriptografada = usuario.FirstOrDefault().Senha;

                if (usuario.Count > 0 && usuario != null && VerifyPassword(Senha, senhaCriptografada, secret))
                {
                    Program.Admin = usuario.FirstOrDefault();
                    return RedirectToAction("Logado", new { id = Program.Admin.ID });
                }
                else
                {
                    return RedirectToAction("Negado");
                }

            } else
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
