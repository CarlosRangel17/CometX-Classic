using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using CometX.Application.Models;

namespace CometX.Application.Interfaces
{
    public interface ICometXManager
    {
        void Delete<T>(T entity) where T : new();
        List<T> GetAll<T>(string query = "") where T : new();
        List<T> GetAll<T>(Expression<Func<T, bool>> expression) where T : new();
        T GetById<T>(int id) where T : new();
        T GetByKey<T>(object key) where T : new();
        List<T> GetSortedTable<T>(SortDirection sortDirection, string sortValue = "", Expression<Func<T, bool>> expression = null) where T : new();
        T FirstOrDefault<T>(Expression<Func<T, bool>> expression = null) where T : new();
        void MarkActive<T>(int id) where T : new();
        void MarkActiveByKey<T>(object key) where T : new();
        void Save<T>(T entity) where T : new();
    }
}
