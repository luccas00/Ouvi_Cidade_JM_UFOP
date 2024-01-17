using System;
using Microsoft.EntityFrameworkCore;
using OuviCidadeJM.Models;

namespace OuviCidadeJM.Context
{
    public class ContatosContext : DbContext
    {

        public ContatosContext(DbContextOptions<ContatosContext> option) : base(option) { }

        public DbSet<Contato> Contatos { get; set; }

    }
}
