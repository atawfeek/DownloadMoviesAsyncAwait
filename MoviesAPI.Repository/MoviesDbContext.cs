using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Domain;
using MoviesAPI.Repository.Entities;

namespace MoviesAPI.Repository
{
    public class MoviesDbContext : DbContext
    {
        public DbSet<MovieEntity> Movies { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }

        public MoviesDbContext(DbContextOptions<MoviesDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieEntity>()
            .HasOne(v => v.Category)
            .WithMany(c => c.Movies)
            .HasForeignKey(p => p.CategoryId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
