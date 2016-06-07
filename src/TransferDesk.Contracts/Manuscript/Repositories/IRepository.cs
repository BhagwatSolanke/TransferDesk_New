using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace TransferDesk.Contracts.Manuscript.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Add(T item);

        T Get(object id);

        T Get(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetAll();

        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetAll(
             Expression<Func<T, bool>> predicate = null,
             params Expression<Func<T, object>>[] includes);

        void Update(T item);

        void Delete(T item);
        
        bool Exists(Expression<Func<T, bool>> predicate = null);

        int Count(Expression<Func<T, bool>> predicate = null);

        // Unit of Work pattern has a common commit so below can be commented

        //void SaveChanges();
    }
}



//public interface IRepository<T> where T : class
//{
//    void Insert(T instance);

//    void Update(T instance);

//    void Remove(T instance);

//    T FirstOrDefault(Expression<Func<T, bool>> predicate = null,
//         params Expression<Func<T, object>>[] includes);

//    IQueryable<T> All(
//         Expression<Func<T, bool>> predicate = null,
//         params Expression<Func<T, object>>[] includes);

//    bool Exists(Expression<Func<T, bool>> predicate = null);

//    int Count(Expression<Func<T, bool>> predicate = null);

//    // This could be removed to require Unit of Work pattern...
//    void SaveChanges();
//}
