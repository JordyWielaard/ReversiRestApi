using Microsoft.EntityFrameworkCore;
using ReversiRestApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.Data
{
    public class SpelDbContext : DbContext
    {
        public SpelDbContext(DbContextOptions<SpelDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Spel>(s =>
            {
                s.HasKey(s => s.ID);
                s.Property(s => s.ID).ValueGeneratedOnAdd();
            });
        }
        public DbSet<Spel> Spel { get; set; }
    }
}
