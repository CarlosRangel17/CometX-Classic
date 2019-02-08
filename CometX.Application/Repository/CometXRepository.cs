using System;
using System.Linq;
using System.Reflection;
using System.Configuration;
using System.Linq.Expressions;
using System.Collections.Generic;
using CometX.Application.Queries;
using CometX.Application.Utilities;
using CometX.Application.Interfaces;
using CometX.Application.Extensions;
using CometX.Application.Extensions.General;

namespace CometX.Application.Repository
{
    public class CometXRepository : ICometXRepository
    {
        #region global variable(s)
        private static string Key { get; set; }
        private static string ConnectionString { get; set; }
        private readonly SqlUtils SqlUtil;
        private static readonly QueryUtils QueryUtil = new QueryUtils();
        #endregion

        #region constructor(s)
        public CometXRepository()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            if (ConnectionString.Contains("metadata")) ConnectionString = ConnectionString.ExtrapolateMetaDataFromConnectionString();
            SqlUtil = new SqlUtils(ConnectionString);
        }

        public CometXRepository(string key = "")
        {
            Key = !string.IsNullOrWhiteSpace(key) ? key : "DefaultConnection";
            ConnectionString = ConfigurationManager.ConnectionStrings[Key].ConnectionString;
            if (ConnectionString.Contains("metadata")) ConnectionString = ConnectionString.ExtrapolateMetaDataFromConnectionString();
            SqlUtil = new SqlUtils(ConnectionString);
        }

        public CometXRepository(string key = "", string connectionString = "")
        {
            Key = key ?? "DefaultConnection";
            ConnectionString = connectionString ?? ConfigurationManager.ConnectionStrings[Key].ConnectionString;
            if (ConnectionString.Contains("metadata")) ConnectionString = ConnectionString.ExtrapolateMetaDataFromConnectionString();
            SqlUtil = new SqlUtils(ConnectionString);
        }
        #endregion

        #region public methods
        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity"></param>
        public void Delete<T>(T entity) where T : new()
        {
            string query = "";

            try
            {
                if (entity == null) throw new ArgumentNullException("entity");

                query = BaseQuery.DELETE_WHERE<T>(entity.ToDeleteQuery());

                SqlUtil.ExecuteDynamicQuery(query);
            }
            catch (Exception ex)
            {
                var message = ex.Message;

                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;

                    message += "\n" + ex.Message;
                }

                throw new Exception(message);
            }
        }

        //TODO: Figure out QueryUtil.Translate()
        /// <summary>
        /// Finds first entity by expression, if any.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public T FirstOrDefault<T>(Expression<Func<T, bool>> expression = null) where T : new()
        {
            try
            {
                string query = expression == null
                    ? BaseQuery.SELECT_FIRST_FROM<T>()
                    : BaseQuery.SELECT_FIRST_FROM<T>(QueryUtil.Translate(expression));

                return SqlUtil.GetInfo<T>(query, null, true);
            }
            catch (Exception ex)
            {
                // Default to query all records, then apply predicate -- Need to figure out the try section
                return SqlUtil.GetMultipleInfo<T>(BaseQuery.SELECT_FROM<T>(), null, true).FirstOrDefault(expression.Compile());
            }
        }

        /// <summary>
        /// Finds entity by id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById<T>(int id) where T : new()
        {
            try
            {
                return SqlUtil.GetInfo<T>(BaseQuery.SELECT_FROM_WHERE<T>("Id = " + id), null, true);
            }
            catch (Exception ex)
            {
                string message = ex.Message;

                while(ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    message += ex.Message;
                }

                throw new Exception(message);
            }
        }

        /// <summary>
        /// Finds entity by key based off column associated with a Primary Key attribute
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetByKey<T>(object key) where T : new()
        {
            try
            {
                return SqlUtil.GetInfo<T>(BaseQuery.SELECT_FROM_WHERE<T>(typeof(T).GetProperties().First(x => x.HasPrimaryKeyAttribute()).Name + " = " + key), null, true);
            }
            catch (Exception ex)
            {
                string message = ex.Message;

                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    message += ex.Message;
                }

                throw new Exception(message);
            }
        }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity"></param>
        public void Insert<T>(T entity) where T : new()
        {
            string query = "";
            try
            {
                if (entity == null) throw new ArgumentNullException("entity");

                query = BaseQuery.INSERT<T>(entity.ToInsertQuery());

                SqlUtil.ExecuteDynamicQuery(query);
            }
            catch (Exception ex)
            {
                var message = ex.Message;

                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;

                    message += "\n" + ex.Message;
                }

                throw new Exception(message);
            }
        }

        //TODO: Figure out QueryUtil.Translate()
        /// <summary>
        /// Returns a list of sorted entities associated with Class T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sortDirection"></param>
        /// <param name="sortValue"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public List<T> SortedTable<T>(string sortDirection, string sortValue, Expression<Func<T, bool>> expression = null) where T : new()
        {
            try
            {
                string query = expression == null
                ? BaseQuery.SELECT_FROM_ORDER_BY<T>(sortDirection, sortValue)
                : BaseQuery.SELECT_FROM_WHERE_ORDER_BY<T>(sortDirection, sortValue, QueryUtil.Translate(expression));

                return SqlUtil.GetMultipleInfo<T>(query, null, true);
            }
            catch (Exception ex)
            {
                // Default to query all records, then apply predicate -- Need to figure out the try section
                return SqlUtil.GetMultipleInfo<T>(BaseQuery.SELECT_FROM_ORDER_BY<T>(sortDirection, sortValue), null, true).Where(expression.Compile()).ToList();
            }
        }

        /// <summary>
        /// Returns a list of all entities associated with class T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<T> Table<T>(string query = "") where T : new()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query)) query = BaseQuery.SELECT_FROM<T>();

                return SqlUtil.GetMultipleInfo<T>(query, null, true);
            }
            catch(Exception ex)
            {
                string message = ex.Message;

                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    message += ex.Message;
                }

                throw new Exception(message);
            }
        }

        //TODO: Figure out QueryUtil.Translate()
        /// <summary>
        /// Returns a list of all entities associated with class T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<T> Table<T>(Expression<Func<T, bool>> expression) where T : new()
        {
            try
            {
                var condition = QueryUtil.Translate(expression);
                return SqlUtil.GetMultipleInfo<T>(BaseQuery.SELECT_FROM_WHERE<T>(condition), null, true);
            }
            catch (Exception ex)
            {
                // Default to query all records, then apply predicate
                return SqlUtil.GetMultipleInfo<T>(BaseQuery.SELECT_FROM_WHERE<T>(), null, true).Where(expression.Compile()).ToList();
            }
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity"></param>
        public void Update<T>(T entity, Expression<Func<T, bool>> expression = null) where T : new()
        {
            string query = "";

            try
            {
                if (entity == null) throw new ArgumentNullException("entity");

                string condition = expression == null ? "" : QueryUtil.Translate(expression);

                query = BaseQuery.UPDATE_WHERE<T>(entity.ToUpdateQuery(condition));

                SqlUtil.ExecuteDynamicQuery(query);
            }
            catch (Exception ex)
            {
                string message = ex.Message;

                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    message += ex.Message;
                }

                throw new Exception(message);
            }
        }
        #endregion

        #region private methods
        #endregion
    }
}
