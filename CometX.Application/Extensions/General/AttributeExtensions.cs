using System.Reflection;
using CometX.Application.Attributes;

namespace CometX.Application.Extensions.General
{
    public static class AttributeExtensions
    {
        public static bool HasAllowIdentityUpdateAttribute(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<AllowIdentityColumnUpdateAttribute>() != null;
        }

        public static bool HasAttributeRestrictions(this PropertyInfo propertyInfo)
        {
            return propertyInfo.HasPropertyNotMappedAttribute() || (propertyInfo.HasPrimaryKeyAttribute() && !propertyInfo.HasAllowIdentityUpdateAttribute());
        }

        public static bool HasDbColumnAttribute(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<DBColumnAttribute>() != null;
        }

        public static bool HasFlagAttribute(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<FlagAttribute>() != null;
        }

        public static bool HasPrimaryKeyAttribute(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<PrimaryKeyAttribute>() != null;
        }

        public static bool HasPropertyNotMappedAttribute(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<PropertyNotMappedAttribute>() != null;
        }

        public static string GetDbColumnAttributeMapping(this PropertyInfo propertyInfo)
        {
            string name = "";

            var columnMapping = propertyInfo.GetCustomAttribute<DBColumnAttribute>();

            if (columnMapping != null) name = (columnMapping as DBColumnAttribute).Name;

            return name;
        }
    }
}
