using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OuviCidadeJM.Models;

namespace OuviCidadeJM_V3.Data;

public class WebAppContext : IdentityDbContext
{
    public WebAppContext(DbContextOptions<WebAppContext> option) : base(option) { }
    public DbSet<Manifestacao> Manifestacao { get; set; }
    public DbSet<Noticia> Noticia { get; set; }
    public DbSet<Cidadao> Cidadao { get; set; }
    public DbSet<Admin> Admin { get; set; }
    public DbSet<Resposta> Resposta { get; set; }
    public DbSet<Secretaria> Secretaria { get; set; }
}
