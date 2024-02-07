using System;
using Microsoft.EntityFrameworkCore;
using OuviCidadeV3.Models;

namespace OuviCidadeV3.Context
{
    public class WebAppContext : DbContext
    {
        public WebAppContext(DbContextOptions<WebAppContext> option) : base(option) { }
        public DbSet<Manifestacao> Manifestacao { get; set; }
        public DbSet<Noticia> Noticia { get; set; }
        public DbSet<Cidadao> Cidadao { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Resposta> Resposta { get; set; }
        public DbSet<Secretaria> Secretaria { get; set; }

    }
}
