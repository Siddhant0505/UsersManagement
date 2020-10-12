using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UsersManagement.DAL.DatabaseEntity;
using UsersManagement.Repository.Contracts.Repository;
using UsersManagement.Repository.Contracts.UnitOfWork;

namespace UsersManagement.Repository.Common
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// It will used in derived classes
        /// </summary>
        protected readonly UserManagementContext Context;

        public Repository(UserManagementContext context)
        {
            Context = context;
        }

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }
        public virtual async Task<ICollection<TEntity>> GetAllAsyn()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }
        /// <summary>
        /// The return type is IEnumerable not IQuerable  
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll()
        {
            lock (this)
            {
                return Context.Set<TEntity>().ToList();
            }
        }


        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).Count();
        }


        public virtual TEntity FindSingle(Expression<Func<TEntity, bool>> match)
        {
            lock (this)
            {

                return Context.Set<TEntity>().SingleOrDefault(match);
            }
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> match)
        {
            lock (this)
            {

                return Context.Set<TEntity>().Any(match);
            }
        }

        public TEntity Get(int id)
        {
            lock (this)
            {
                return Context.Set<TEntity>().Find(id);
            }
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }
        public void RemoveKey(int key)
        {
            Context.Set<TEntity>().Remove(Context.Set<TEntity>().Find(key));
        }
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }
        public virtual TEntity Update(TEntity t, object key)
        {
            if (t == null)
                return null;
            TEntity exist = Context.Set<TEntity>().Find(key);
            if (exist != null)
            {
                Context.Entry(exist).CurrentValues.SetValues(t);
                Context.SaveChanges();
            }
            return exist;
        }
        public virtual TEntity UpdateTrans(TEntity t, object key)
        {
            if (t == null)
                return null;
            TEntity exist = Context.Set<TEntity>().Find(key);
            if (exist != null)
            {
                Context.Entry(exist).CurrentValues.SetValues(t);
            }
            return exist;
        }
        public virtual async Task<TEntity> AddAsyn(TEntity t)
        {

            Context.Set<TEntity>().Add(t);
            await Context.SaveChangesAsync();
            return t;

        }
        public virtual void UpdateColumn(TEntity entity, params Expression<Func<TEntity, object>>[] updatedProperties)
        {
            //dbEntityEntry.State = EntityState.Modified; --- I cannot do this.

            //Ensure only modified fields are updated.
            var dbEntityEntry = Context.Entry(entity);
            if (updatedProperties.Any())
            {
                //update explicitly mentioned properties
                foreach (var property in updatedProperties)
                {
                    dbEntityEntry.Property(property).IsModified = true;
                }
            }
            else
            {
                //no items mentioned, so find out the updated entries
                foreach (var property in dbEntityEntry.OriginalValues.Properties)
                {
                    var original = dbEntityEntry.OriginalValues.GetValue<object>(property);
                    var current = dbEntityEntry.CurrentValues.GetValue<object>(property);
                    if (original != null && !original.Equals(current))
                        dbEntityEntry.Property(property.Name).IsModified = true;
                }
            }
        }


        //public IEnumerable<TEntity> ExecWithStoreProcedure(string query, params object[] parameters)
        //{
        //    Context.Database.
        //    return Context.Database.SqlQuery<TEntity>(query, parameters);
        //}
    }
}
