using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PortolWeb.Entities
{
    public interface IRepositoryBase<T> where T : class, new()
    {

        T Get(Guid id);
        T Get(Expression<Func<T, bool>> expression);

        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Expression<Func<T, bool>> expression);

        //  AsyncTableQuery<T> AsQueryable();
        void Insert(T entity);
        void Insert(IEnumerable<T> entities);
        void Update(T entity);
        void Update(IEnumerable<T> entities);
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
    }
}
