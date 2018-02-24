using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.ApplicationBlocks.Data;

namespace CometX.Application.Utilities
{
    public class SqlUtils
    {
        private string ConnectionString { get; set; }

        public SqlUtils()
        {

        }

        public SqlUtils(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public bool CheckStoredProc(string storedProc, Dictionary<string, string> parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = storedProc;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;

                foreach (var parameter in parameters ?? new Dictionary<string, string>())
                {
                    cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Bit);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                conn.Open();
                cmd.ExecuteNonQuery();

                return (int)returnParameter.Value != 0;
            }
        }

        public void ExecuteDynamicQuery(string query)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ExecuteStoredProc(string storedProc, Dictionary<string, string> parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = storedProc;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;

                foreach (var parameter in parameters ?? new Dictionary<string, string>())
                {
                    cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<T> GetMultipleInfo<T>(string query, SqlParameter[] parameters = null, bool IsQuery = false) where T : new()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                IDataReader reader = IsQuery
                    ? SqlHelper.ExecuteReader(conn, CommandType.Text, query)
                    : SqlHelper.ExecuteReader(conn, query, parameters);
                return MappingUtils.MapMultiple<T>(reader);
            }
        }

        public T GetInfo<T>(string query, SqlParameter[] parameters = null, bool IsQuery = false) where T : new()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                IDataReader reader = IsQuery
                   ? SqlHelper.ExecuteReader(conn, CommandType.Text, query)
                   : SqlHelper.ExecuteReader(conn, query, parameters);
                return MappingUtils.MapFirstSingle<T>(reader);
            }
        }
    }
}
