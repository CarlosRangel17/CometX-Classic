using System;
using System.Linq;
using System.Reflection;
using System.Data.SqlClient;
using System.Data.EntityClient;
using System.Collections.Generic;
using CometX.Application.Extensions.General;

namespace CometX.Application.Extensions
{
    public static class RepositoryExtension
    {
        public static string ExtrapolateMetaDataFromConnectionString(this string connectionString)
        {
            EntityConnectionStringBuilder entityConnectionStringBuilder = new EntityConnectionStringBuilder(connectionString);
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder(entityConnectionStringBuilder.ProviderConnectionString);
            return "data source=" + connectionStringBuilder.DataSource + ";initial catalog=" + connectionStringBuilder.InitialCatalog + ";integrated security=True;";
        }

        public static object[] ToDeleteQuery<T>(this T entity, string condition = "") where T : new()
        {
            var args = new List<object>();

            // Write Table to Delete
            args.Add(entity.GetType().Name.ToString());

            // Retrieve properties
            var properties = typeof(T).GetProperties().ToList();

            // Write Where condition
            if (string.IsNullOrWhiteSpace(condition)) condition = properties.ToDefaultWhereCondition(entity);
            args.Add(condition.TrimEndAllSpaceWithAndPersands().ParseToSQLSyntax());

            return args.ToArray();
        }

        public static object[] ToInsertQuery<T>(this T entity) where T : new()
        {
            var args = new List<object>();

            // Write Table to insert into
            args.Add(entity.GetType().Name.ToString());

            // Retrieve properties & type
            var properties = typeof(T).GetProperties().ToList();
            Type baseType = typeof(T);

            // Write Parameters to insert
            args.Add(string.Format("{0}", (properties.Aggregate("", (message, property) => message + (baseType.GetProperty(property.Name).HasAttributeRestrictions() ? "" : property.Name + ", "))).TrimEndAllSpaceAndCommas()));

            // Write Values to insert
            var arg3 = string.Format("{0}", (properties.Aggregate("", (message, property) => message + (property.HasAttributeRestrictions() ? "" : property.GetEntityValue(entity) + ", ")).TrimEndAllSpaceAndCommas()));
            args.Add(arg3);

            return args.ToArray();
        }

        public static object[] ToUpdateQuery<T>(this T entity, string condition = "") where T : new()
        {
            var args = new List<object>();

            // Write Table to update into
            args.Add(entity.GetType().Name.ToString());

            // Retrieve properties
            var properties = typeof(T).GetProperties().ToList();

            // Write Parameters to update
            string query = "";
            Type baseType = typeof(T);
            foreach (var prop in properties)
            {
                PropertyInfo propertyInfo = baseType.GetProperty(prop.Name);

                if (propertyInfo.HasAttributeRestrictions()) continue;

                query += prop.Name + " = " + prop.GetEntityValue(entity) + ", ";
            }

            args.Add(query.TrimEndAllSpaceAndCommas());

            // Write Where condition
            if (string.IsNullOrWhiteSpace(condition)) condition = properties.ToDefaultWhereCondition(entity);
            args.Add(condition.TrimEndAllSpaceWithAndPersands().ParseToSQLSyntax());

            return args.ToArray();
        }
    }
}
