﻿using CometX.Application.Extensions.General;

namespace CometX.Application.Queries
{
    public class BaseQuery
    {
        public static string SELECT_FIRST_FROM_QUERY = "SELECT TOP 1 * FROM [{0}]";
        public static string SELECT_FIRST_FROM_WHERE_QUERY = "SELECT TOP 1 * FROM [{0}] WHERE {1}";
        public static string SELECT_FROM_QUERY = "SELECT * FROM [{0}]";
        public static string SELECT_FROM_ORDER_BY_QUERY = "SELECT * FROM [{0}] ORDER BY {1} {2}";
        public static string SELECT_FROM_WHERE_QUERY = "SELECT * FROM [{0}] WHERE {1}";
        public static string SELECT_FROM_WHERE_EXISTS_QUERY = "SELECT CASE WHEN EXISTS (SELECT * FROM {0} WHERE {1}) THEN 1 ELSE 0 END AS 'Result'";
        public static string SELECT_FROM_WHERE_ORDER_BY_QUERY = "SELECT * FROM [{0}] WHERE {1} ORDER BY {2} {3}";
        public static string DELETE_WHERE_QUERY = "DELETE FROM [{0}] WHERE {1}";
        public static string DELETE_FIRST_WHERE_QUERY = "DELETE TOP (1) FROM [{0}] WHERE {1}";
        public static string INSERT_QUERY = "INSERT INTO [{0}] ({1}) VALUES({2})";
        public static string ORDER_BY_QUERY = "ORDER BY {0} {1}";
        public static string UPDATE_ALL_QUERY = "UPDATE [{0}] SET {1}";
        public static string UPDATE_WHERE_QUERY = "UPDATE [{0}] SET {1} WHERE {2}";

        public static string APPEND_FETCH(string query, int recordsToTake)
        {
            return query + string.Format(" FETCH NEXT {0} ROWS ONLY", recordsToTake);
        }

        public static string APPEND_SKIP(string query, int recordsToSkip)
        {
            return query + string.Format(" OFFSET {0} ROWS", recordsToSkip);
        }

        public static string DELETE_WHERE<T>(object[] parameters)
        {
            return string.Format(DELETE_WHERE_QUERY, parameters);
        }

        public static string DELETE_FIRST_WHERE<T>(object[] parameters)
        {
            return string.Format(DELETE_FIRST_WHERE_QUERY, parameters);
        }

        public static string INSERT<T>(object[] parameters)
        {
            return string.Format(INSERT_QUERY, parameters);
        }

        public static string SELECT_FIRST_FROM<T>(string condition = "")
        {
            if (string.IsNullOrWhiteSpace(condition)) return string.Format(SELECT_FIRST_FROM_QUERY, typeof(T).Name);

            return string.Format(SELECT_FIRST_FROM_WHERE_QUERY, typeof(T).Name, condition);
        }

        public static string SELECT_FROM<T>()
        {
            return string.Format(SELECT_FROM_QUERY, typeof(T).Name);
        }

        public static string SELECT_FROM_ORDER_BY<T>(string sortDirection = "", string sortValue = "")
        {
            if (string.IsNullOrWhiteSpace(sortDirection) || string.IsNullOrWhiteSpace(sortValue)) return SELECT_FROM<T>();

            return string.Format(SELECT_FROM_ORDER_BY_QUERY, typeof(T).Name, sortValue, sortDirection);
        }

        public static string SELECT_FROM_WHERE<T>(string condition = "")
        {
            if (string.IsNullOrWhiteSpace(condition)) return string.Format(SELECT_FROM_QUERY, typeof(T).Name);

            return string.Format(SELECT_FROM_WHERE_QUERY, typeof(T).Name, condition);
        }

        public static string SELECT_FROM_WHERE_EXISTS<T>(string condition)
        {
            return string.Format(SELECT_FROM_WHERE_EXISTS_QUERY, typeof(T).Name, condition);
        }

        public static string SELECT_FROM_WHERE_ORDER_BY<T>(string sortDirection, string sortValue, string condition = "")
        {
            if (string.IsNullOrWhiteSpace(condition)) return string.Format(SELECT_FROM_ORDER_BY_QUERY, typeof(T).Name, sortValue, sortDirection);

            return string.Format(SELECT_FROM_WHERE_ORDER_BY_QUERY, typeof(T).Name, condition, sortValue, sortDirection);

        }
        public static string UPDATE_WHERE<T>(object[] parameters)
        {
            return string.Format(UPDATE_WHERE_QUERY, parameters);
        }
    }
}
