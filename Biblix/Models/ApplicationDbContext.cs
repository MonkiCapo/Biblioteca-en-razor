using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Biblix.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Reemplazar con su información
            var connectionString = "server=localhost;database=FEDERICO;user=5to_agbd;password=Trigg3rs!;";
            var serverVersion = ServerVersion.Parse("8.0.41");

            optionsBuilder.UseMySql(connectionString, serverVersion);
        }

        /* protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        } */
    }
}