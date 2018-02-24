using System;
using System.Globalization;
using System.Linq.Expressions;

namespace CometX.Application.Extensions.General
{
    public static class StringExtension
    {
        public static bool ConvertBitStringToBool(this string genericString)
        {
            return genericString.Equals("1");
        }

        //TODO: Need to test
        public static string ConvertLambdaToSQLQuery<T>(this Expression<Func<T, bool>> expression)
        {
            string body = expression.Body.ToString();

            foreach (var parm in expression.Parameters)
            {
                var parmName = parm.Name;
                var parmTypeName = parm.Type.Name;
                body = body.Replace(parmName + ".", parmTypeName + ".");
            }

            return body;
        }

        public static string ConvertYearDayMonthToMonthDayYear(this string date)
        {
            DateTime d = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            return d.ToString("MM/dd/yyyy");
        }

        public static string ParseToSQLSyntax(this string genericString)
        {
            return genericString.Replace("&&", "AND").Replace("||", "OR").Replace("!=", "<>");
        }

        public static string TrimEndAllSpaceAndCommas(this string genericString)
        {
            return genericString.TrimEnd(' ').TrimEnd(',');
        }

        public static string TrimEndAllSpaceWithAndPersands(this string genericString)
        {
            return genericString.TrimEnd(' ').TrimEnd('&');
        }

        public static string TrimEndAllSpaceWithDoubleAndPersands(this string genericString)
        {
            return genericString.TrimEnd(' ').TrimEnd('&').Trim('&');
        }
    }
}
