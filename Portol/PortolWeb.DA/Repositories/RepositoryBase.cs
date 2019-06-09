using Microsoft.EntityFrameworkCore;
using PortolWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PortolWeb.DA.Repositories
{
   public class RepositoryBase<T> : IDisposable, IRepositoryBase<T> where T : class, new()
    {


        DataContext _context;

        public RepositoryBase(DataContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

       
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>();           
        }


        public IEnumerable<T> GetAll(Expression<Func<T, bool>> expression)
        {
            var result = _context.Set<T>().Where(expression);
            //if (result != null)
            //{
                
            //    _context.Entry(result).State = EntityState.Detached;
            //}
            return result;
        }

        public virtual T Get(Guid id)
        {          
            var result= _context.Find<T>(id);
            if(result!=null)
            {
                _context.Entry(result).State = EntityState.Detached;
            }            
            return result;
        }

        public virtual T Get(Expression<Func<T, bool>> expression)
        {           
            var result= _context.Set<T>().Where(expression).FirstOrDefault();
            if (result != null)
            {
                _context.Entry(result).State = EntityState.Detached;
            }
            return result;
        }

        public virtual void Insert(T entity)
        {
            this._context.Set<T>().Add(entity);
        }

        public virtual void Insert(IEnumerable<T> entities)
        {
            this._context.Set<T>().AddRange(entities);
        }

        public virtual void Update(T entity)
        {
            this._context.Set<T>().Update (entity);
        }

        public virtual void Update(IEnumerable<T> entities)
        {
            this._context.Set<T>().UpdateRange(entities);
        }

        public virtual void Delete(T entity)
        {
            this._context.Set<T>().Remove(entity);
        }
        public virtual void Delete(IEnumerable<T> entities)
        {
            this._context.Set<T>().RemoveRange(entities);
        }
               

        public void Dispose()
        {
            _context.Dispose();           
        }
    }
}
