using System;
using System.ComponentModel.DataAnnotations;

namespace CometX.Application.Extensions.General
{
    public static class EnumExtension
    {
        /// <summary>
        /// Gets the enum description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToDescription(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DisplayAttribute[])fi.GetCustomAttributes(typeof(DisplayAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Name;
            else
                return value.ToString();
        }
    }
}
