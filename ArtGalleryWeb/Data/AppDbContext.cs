using ArtGalleryWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtGalleryWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 
            
        }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          modelBuilder.Entity<Category>().HasData(
              new Category { Id = 1, Name = "Sculpture", DisplayOrder = 7 },
              new Category { Id = 2, Name = "Painting", DisplayOrder = 1 },
              new Category { Id = 3, Name = "Drawing", DisplayOrder = 3 },
              new Category { Id = 4, Name = "Photography", DisplayOrder = 4 }
              );
        }
    }
}
