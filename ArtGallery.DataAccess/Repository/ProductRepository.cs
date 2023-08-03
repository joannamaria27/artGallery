using ArtGallery.DataAcess.Data;
using ArtGallery.DataAcess.Repository.IRepository;
using ArtGallery.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.DataAcess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {


        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }
    }
}

