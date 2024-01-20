using System;
using Microsoft.EntityFrameworkCore;
using OuviCidadeJM.Models;

namespace OuviCidadeJM.Context
{
    public class WebAppContext : DbContext
    {

        public WebAppContext(DbContextOptions<WebAppContext> option) : base(option) { }

        public DbSet<Contato> Contatos { get; set; }
        public DbSet<Manifestacao> Manifestacoes { get; set; }
        public DbSet<Noticia> Noticias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Log> Logs { get; set; }

    }
}
