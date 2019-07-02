using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using CometX.Application.Models;
using CometX.Application.Interfaces;
using CometX.Application.Repository;
using CometX.Application.Extensions.General;

namespace CometX.Application.Managers
{
    public class CometXManager : ICometXManager
    {
        #region global variable(s)
        protected internal CometXRepository _repo;
        #endregion

        #region constructor(s)
        public CometXManager(string key = "")
        {
            _repo = new CometXRepository(key);
        }
        #endregion

        #region public methods
        /// <summary>
        /// Queries the specified domain by expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public bool Any<T>(Expression<Func<T, bool>> expression)
        {
            return _repo.CheckTable(expression);
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Delete<T>(T entity) where T : new()
        {
            _repo.Delete(entity);
        }

        /// <summary>
        /// Deletes all specified entities based off provided expression. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void DeleteAll<T>(Expression<Func<T, bool>> expression)
        {
            _repo.Delete(expression, true);
        }

        /// <summary>
        /// Deletes the first entity based off provided expression. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void DeleteFirst<T>(Expression<Func<T, bool>> expression)
        {
            _repo.Delete<T>(expression);
        }

        /// <summary>
        /// Returns a list of all entities associated with class T based off string query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<T> GetAll<T>(string query = "") where T : new()
        {
            return _repo.Table<T>(query);
        }

        /// <summary>
        /// Returns a list of all entities associated with class T based off expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public List<T> GetAll<T>(Expression<Func<T, bool>> expression) where T : new()
        {
            return _repo.Table(expression);
        }

        /// <summary>
        /// Finds entity by id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById<T>(int id) where T : new()
        {
            return _repo.GetById<T>(id);
        }

        /// <summary>
        /// Finds entity by key based off column associated with a Primary Key attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetByKey<T>(object key) where T : new()
        {
            return _repo.GetByKey<T>(key);
        }

        /// <summary>
        /// Returns a list of sorted entities associated with Class T based off expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sortDirection"></param>
        /// <param name="sortValue"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public List<T> GetSortedTable<T>(SortDirection sortDirection, string sortValue, Expression<Func<T, bool>> expression = null) where T : new()
        {
            return _repo.SortedTable(sortDirection.ToDescription(), sortValue, null, null, expression);
        }

        /// <summary>
        /// Returns a list of sorted entities associated with Class T based off expression and/or skip/take parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sortDirection"></param>
        /// <param name="sortValue"></param>
        /// <param name="recordsToSkip"></param>
        /// <param name="recordsToTake"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public List<T> GetSortedTable<T>(SortDirection sortDirection, string sortValue, int recordsToSkip, int? recordsToTake = null, Expression<Func<T, bool>> expression = null) where T : new()
        {
            return _repo.SortedTable(sortDirection.ToDescription(), sortValue, recordsToSkip, recordsToTake, expression);
        }

        /// <summary>
        /// Finds first entity by expression, if any.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public T FirstOrDefault<T>(Expression<Func<T, bool>> expression = null) where T : new()
        {
            return _repo.FirstOrDefault(expression);
        }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Insert<T>(T entity) where T : new()
        {
            _repo.Insert(entity);
        }

        /// <summary>
        /// Inserts & Returns object context. Note that this function must only be used for entities with PK column 'Id'  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void InsertWithContext<T>(ref T entity) where T : new()
        {
            _repo.Insert(entity);
            entity = _repo.SortedTable<T>(SortDirection.Descending.ToDescription(), "Id", 0, 1).FirstOrDefault();
        }

        /// <summary>
        /// Finds entity by Id and marks active / inactive the column associated with a Flag attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        public void MarkActive<T>(int id) where T : new()
        {
            _repo.Update(MarkPropertyActive(_repo.GetById<T>(id)));
        }

        /// <summary>
        /// Finds entity by Key and marks active / inactive the column associated with a Flag attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public void MarkActiveByKey<T>(object key) where T : new()
        {
            _repo.Update(MarkPropertyActive(_repo.GetByKey<T>(key)));
        }

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Save<T>(T entity) where T : new()
        {
            var type = entity.GetType();

            if (typeof(T).GetProperties().Any(x => x.HasPrimaryKeyAttribute()))
            {
                bool insert = false;
                var key = typeof(T).GetProperties().First(x => x.HasPrimaryKeyAttribute());

                if (key.IsLongType())
                {
                    insert = Convert.ToInt64(key.GetEntityValue(entity)) == 0;
                }
                if (key.IsIntType())
                {
                    insert = Convert.ToInt64(key.GetEntityValue(entity)) == 0;
                }
                if (key.IsStringType())
                {
                    insert = string.IsNullOrWhiteSpace(key.GetEntityValue(entity));
                }

                if (insert)
                {
                    _repo.Insert(entity);
                }
                else
                {
                    _repo.Update(entity);
                }
            }
            else
            {
                var value = type.GetProperty("Id").GetValue("Id");
                if (value == null || (int)value == 0)
                {
                    _repo.Insert(entity);
                }
                else
                {
                    _repo.Update(entity);
                }
            }
        }

        /// <summary>
        /// Updates the specified entity based off PrimaryKey attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Update<T>(T entity) where T : new()
        {
            _repo.Update(entity);
        }

        /// <summary>
        /// Updates the specified entity based off provided expression. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Update<T>(T entity, Expression<Func<T, bool>> expression) where T : new()
        {
            _repo.Update(entity, expression);
        }
        #endregion

        #region private methods
        /// <summary>
        /// Sets the flag property 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="record"></param>
        /// <returns></returns>
        private object MarkPropertyActive<T>(T record) where T : new()
        {
            var propertyInfo = typeof(T).GetProperties().First(x => x.HasFlagAttribute());
            var currentValue = propertyInfo.GetEntityValue(record).ConvertBitStringToBool();
            propertyInfo.SetValue(record, currentValue, propertyInfo.GetIndexParameters());
            return record;
        }
        #endregion
    }
}
