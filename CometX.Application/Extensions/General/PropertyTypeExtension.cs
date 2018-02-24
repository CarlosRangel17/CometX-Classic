using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace CometX.Application.Extensions.General
{
    public static class PropertyTypeExtension
    {
        public static string GetEntityValue<T>(this PropertyInfo property, T entity, bool allowRestrictions = false) where T : new()
        {
            Type baseType = typeof(T);

            if (property.HasAttributeRestrictions() && !allowRestrictions) return "";

            var propertyVal = property.GetValue(entity);

            if (propertyVal != null && (property.IsDateTimeType() || property.IsStringType())) propertyVal = "'" + propertyVal + "'";
            else if (propertyVal == null && property.IsNullableType()) propertyVal = "NULL";
            else if (property.IsStringType()) propertyVal = "''";
            else if (property.IsEnumType()) propertyVal = (int)propertyVal;
            else if (property.IsBoolType()) propertyVal = (bool)propertyVal ? '1' : '0';

            return Convert.ToString(propertyVal);
        }

        public static bool HasProperty(this Type obj, string propertyName)
        {
            return obj.GetProperty(propertyName) != null;
        }

        public static bool IsBoolType(this PropertyInfo property)
        {
            return property.PropertyType.Name.ToLower().Equals("boolean") || property.PropertyType.Name.ToLower().Equals("bool");
        }

        public static bool IsDateTimeType(this PropertyInfo property)
        {
            return property.PropertyType.Name.ToLower().Equals("datetime");
        }

        public static bool IsDoubleType(this PropertyInfo property)
        {
            return property.PropertyType.Name.ToLower().Equals("double");
        }

        public static bool IsEnumType(this PropertyInfo property)
        {
            return property.PropertyType.IsEnum;
        }

        public static bool IsLongType(this PropertyInfo property)
        {
            return property.PropertyType.Name.ToLower().Equals("int64");
        }

        public static bool IsIntType(this PropertyInfo property)
        {
            return property.PropertyType.Name.ToLower().Equals("int32");
        }

        public static bool IsNullableType(this PropertyInfo property)
        {
            return property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsStringType(this PropertyInfo property)
        {
            return property.PropertyType.Name.ToLower().Equals("string");
        }

        public static string ToDefaultWhereCondition<T>(this List<PropertyInfo> properties, T entity, bool includeWhereClause = false) where T : new()
        {
            if (!properties.Any(x => x.HasPrimaryKeyAttribute())) return "";

            var prestatement = includeWhereClause ? "WHERE " : "";

            var primaryKeys = properties.Where(x => x.HasPrimaryKeyAttribute()).ToList();

            primaryKeys.ForEach(x => prestatement += x.Name + " = " + x.GetEntityValue(entity, true) + " && ");

            return prestatement;
        }
    }
}
