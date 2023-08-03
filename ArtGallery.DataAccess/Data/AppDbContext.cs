using ArtGallery.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.DataAcess.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 
            
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          modelBuilder.Entity<Category>().HasData(
              new Category { Id = 1, Name = "Sculpture", DisplayOrder = 7 },
              new Category { Id = 2, Name = "Painting", DisplayOrder = 1 },
              new Category { Id = 3, Name = "Drawing", DisplayOrder = 3 },
              new Category { Id = 4, Name = "Photography", DisplayOrder = 4 }
              );

            modelBuilder.Entity<Product>().HasData(
              new Product { 
                  Id = 1, 
                  Title = "Dream", 
                  Author="Al B.", 
                  CreatedDate=new DateTime(2020,12,03), 
                  Description= "My oil painting of a landscape with my house. Size 2x1m.", 
                  Price=2345, 
                  StockQuantity=1
              },
              new Product
              {
                  Id = 2,
                  Title = "Biography",
                  Author = "Tom Nowak",
                  CreatedDate = new DateTime(2023, 02, 01),
                  Description = "My biography. 245 pages.",
                  Price = 55,
                  StockQuantity = 10000
              }
              );
        }
    }
}
