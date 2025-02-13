﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Eticaret.Core.Entities;

namespace Eticaret.Service.Abstract
{
    public interface IService<T> where T : class, IEntity, new()
    {
        List<T> GetAll();
        List<T> GetAll(Expression<Func<T,bool>> expression);
        IQueryable<T> GetQueryable();
        T Get(Expression<Func<T, bool>> expression);
        T Find(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        int SaveChanges();

        // Asenkron metotlar

        Task<T> FindAsync(int id);
        Task<T> GetAsync(Expression<Func<T, bool>> expression);
        Task <List<T>> GetAllAsync(Expression<Func<T, bool>> expression);
        Task <List<T>> GetAllAsync();

        Task AddAsync(T entity);
        Task<int> SaveChangesAsync();
    }
}
