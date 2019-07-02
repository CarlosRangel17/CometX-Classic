using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using InternalDataMigration.DataMigration.Attributes;

namespace InternalDataMigration.DataMigration.Utilities
{
    public class MappingUtils
    {
        public static T MapFirstSingle<T>(IDataReader reader) where T : new()
        {
            return Map<T>(reader).SingleOrDefault();
        }

        public static List<T> MapMultiple<T>(IDataReader reader) where T : new()
        {
            return Map<T>(reader);
        }

        public static object MapObject(IDataReader reader)
        {
            var list = new List<Dictionary<string, object>>();

            var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName);

            while (reader.Read())
            {
                var dict = new Dictionary<string, object>();

                foreach (var column in columns)
                {
                    var propertyVal = reader[column];

                    dict.Add(column, propertyVal);
                }

                list.Add(dict);
            }

            return list;
        }

        private static List<T> Map<T>(IDataReader reader) where T : new()
        {
            List<PropertyInfo> properties = typeof(T).GetProperties().ToList();

            Type baseType = typeof(T);
            List<T> result = new List<T>();
            while (reader.Read())
            {
                T entry = new T();

                foreach (var property in properties)
                {
                    try
                    {
                        PropertyInfo propertyInfo = baseType.GetProperty(property.Name);

                        if (propertyInfo.GetCustomAttribute<PropertyNotMappedAttribute>() != null) continue;

                        //find the property type
                        Type propertyType = propertyInfo.PropertyType;

                        //if the property type is nullable, we need to get the underlying type of the property
                        var targetType = IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType;

                        //Returns an System.Object with the specified System.Type and whose value is equivalent to the specified object.
                        var propertyVal = reader[property.Name];

                        if (!(propertyVal is DBNull))
                        {
                            propertyVal = propertyInfo.PropertyType.IsEnum ? Enum.ToObject(targetType, propertyVal) : Convert.ChangeType(propertyVal, targetType);
                        }

                        if ((propertyVal is DBNull && IsNullableType(propertyInfo.PropertyType)))
                        {
                            propertyVal = null;
                        }

                        if ((propertyVal is DBNull && IsStringType(propertyInfo.PropertyType)))
                        {
                            propertyVal = "";
                        }

                        //Set the value of the property
                        propertyInfo.SetValue(entry, propertyVal, property.GetIndexParameters());
                    }
                    catch (Exception ex)
                    {
                        var message = "Map error: " + ex.Message;

                        while (ex.InnerException != null)
                        {
                            ex = ex.InnerException;
                            message += ex.Message;
                        }

                        throw new Exception(message);
                    }
                }

                result.Add(entry);
            }

            return result;
        }

        private static bool IsNullableType(Type propertyType)
        {
            return propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static bool IsStringType(Type propertyType)
        {
            return propertyType.Name.ToLower().Equals("string");
        }
    }
}
