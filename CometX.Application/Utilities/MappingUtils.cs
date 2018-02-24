using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using CometX.Application.Attributes;
using CometX.Application.Extensions.General;

namespace CometX.Application.Utilities
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

                        //if the property type is nullable, we need to get the underlying type of the property
                        var targetType = propertyInfo.IsNullableType() ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType;

                        //Returns an System.Object with the specified System.Type and whose value is equivalent to the specified object.
                        var propertyVal = reader[property.Name];

                        if (!(propertyVal is DBNull))
                        {
                            propertyVal = propertyInfo.PropertyType.IsEnum ? Enum.ToObject(targetType, propertyVal) : Convert.ChangeType(propertyVal, targetType);
                        }

                        if (propertyVal is DBNull && propertyInfo.IsNullableType())
                        {
                            propertyVal = null;
                        }

                        if (propertyVal is DBNull && propertyInfo.IsStringType())
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
    }
}
