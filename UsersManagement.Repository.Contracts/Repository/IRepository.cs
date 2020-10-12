﻿using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UsersManagement.Repository.Contracts.Repository
{
    /// <summary>
    /// This interfacce have three groups Find,Add and Remove
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// Generic interface where TEntity is a class
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        Task<ICollection<TEntity>> GetAllAsyn();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        bool Any(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveKey(int key);
        void RemoveRange(IEnumerable<TEntity> entities);
        TEntity Update(TEntity t, object key);
        TEntity UpdateTrans(TEntity t, object key);
        Task<TEntity> AddAsyn(TEntity t);
        int Count(Expression<Func<TEntity, bool>> predicate);
        void UpdateColumn(TEntity t, params Expression<Func<TEntity, object>>[] updatedProperties);

    }
}
