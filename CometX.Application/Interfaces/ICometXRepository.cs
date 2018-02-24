using System;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace CometX.Application.Interfaces
{
    public interface ICometXRepository
    {
        void Delete<T>(T entity) where T : new();
        void Insert<T>(T entity) where T : new();
        T GetById<T>(int id) where T : new();
        T FirstOrDefault<T>(Expression<Func<T, bool>> expression = null) where T : new();
        List<T> SortedTable<T>(string sortDirection, string sortValue, Expression<Func<T, bool>> expression = null) where T : new();
        List<T> Table<T>(string query = "") where T : new();
        List<T> Table<T>(Expression<Func<T, bool>> expression) where T : new();
        void Update<T>(T entity) where T : new();
    }
}
