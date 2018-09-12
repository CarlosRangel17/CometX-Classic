using CometX.Application.Extensions.General;

namespace CometX.Application.Queries
{
    public class BaseQuery
    {
        public static string SELECT_FIRST_FROM_QUERY = "SELECT TOP 1 * FROM [{0}]";
        public static string SELECT_FIRST_FROM_WHERE_QUERY = "SELECT TOP 1 * FROM [{0}] WHERE {1}";
        public static string SELECT_FROM_QUERY = "SELECT * FROM [{0}]";
        public static string SELECT_FROM_ORDER_BY_QUERY = "SELECT * FROM [{0}] ORDER BY {1} {2}";
        public static string SELECT_FROM_WHERE_QUERY = "SELECT * FROM [{0}] WHERE {1}";
        public static string SELECT_FROM_WHERE_ORDER_BY_QUERY = "SELECT * FROM [{0}] WHERE {1} ORDER BY {2} {3}";
        public static string DELETE_WHERE_QUERY = "DELETE FROM [{0}] WHERE {1}";
        public static string INSERT_QUERY = "INSERT INTO [{0}] ({1}) VALUES({2})";
        public static string ORDER_BY_QUERY = "ORDER BY {0} {1}";
        public static string UPDATE_ALL_QUERY = "UPDATE [{0}] SET {1}";
        public static string UPDATE_WHERE_QUERY = "UPDATE [{0}] SET {1} WHERE {2}";

        public static string DELETE_WHERE<T>(object[] parameters) where T : new()
        {
            return string.Format(DELETE_WHERE_QUERY, parameters);
        }

        public static string INSERT<T>(object[] parameters) where T : new()
        {
            return string.Format(INSERT_QUERY, parameters);
        }

        public static string SELECT_FIRST_FROM<T>(string condition = "") where T : new()
        {
            if (string.IsNullOrWhiteSpace(condition)) return string.Format(SELECT_FIRST_FROM_QUERY, typeof(T).Name);

            return string.Format(SELECT_FIRST_FROM_WHERE_QUERY, typeof(T).Name, condition);
        }

        public static string SELECT_FROM<T>() where T : new()
        {
            return string.Format(SELECT_FROM_QUERY, typeof(T).Name);
        }

        public static string SELECT_FROM_ORDER_BY<T>(string sortDirection = "", string sortValue = "") where T : new()
        {
            if (string.IsNullOrWhiteSpace(sortDirection) || string.IsNullOrWhiteSpace(sortValue)) return SELECT_FROM<T>();

            return string.Format(SELECT_FROM_ORDER_BY_QUERY, typeof(T).Name, sortValue, sortDirection);
        }

        public static string SELECT_FROM_WHERE<T>(string condition = "")
        {
            if (string.IsNullOrWhiteSpace(condition)) return string.Format(SELECT_FROM_QUERY, typeof(T).Name);

            return string.Format(SELECT_FROM_WHERE_QUERY, typeof(T).Name, condition);
        }

        public static string SELECT_FROM_WHERE_ORDER_BY<T>(string sortDirection, string sortValue, string condition = "")
        {
            if (string.IsNullOrWhiteSpace(condition)) return string.Format(SELECT_FROM_ORDER_BY_QUERY, typeof(T).Name, sortValue, sortDirection);

            return string.Format(SELECT_FROM_WHERE_ORDER_BY_QUERY, typeof(T).Name, condition, sortValue, sortDirection);

        }
        public static string UPDATE_WHERE<T>(object[] parameters) where T : new()
        {
            return string.Format(UPDATE_WHERE_QUERY, parameters);
        }
    }
}
