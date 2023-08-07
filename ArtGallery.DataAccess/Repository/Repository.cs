using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ArtGallery.DataAcess.Data;
using ArtGallery.DataAcess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.DataAcess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        internal DbSet<T> dbSet;
        public Repository(AppDbContext context)
        {
            _context = context;
            this.dbSet = _context.Set<T>();
            _context.Products.Include(u => u.Category).Include(u=>u.CategoryId);
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }
        public T Get(Expression<Func<T, bool>> filter, string? includePropertis = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includePropertis))
            {
                foreach (var propertis in includePropertis.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(propertis);
                }
            }
            return query.FirstOrDefault();
        }
        public IEnumerable<T> GetAll(string? includePropertis = null)
        {
            IQueryable<T> query = dbSet;
            if(!string.IsNullOrEmpty(includePropertis))
            {
                foreach(var propertis in includePropertis.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(propertis);
                }
            }
            return query.ToList();
        }
        public void Remove(T entity)
        {
           dbSet.Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entitie)
        {
            dbSet.RemoveRange(entitie);
        }
    }
}
