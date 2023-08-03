﻿using System;
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
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }
        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }
        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
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