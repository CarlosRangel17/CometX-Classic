using System;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace CometX.Application.Utilities
{
    public class QueryUtils : ExpressionVisitor
    {
        private StringBuilder sb;
        private string _orderBy = string.Empty;
        //private string _any = string.Empty;
        private string _contains = string.Empty;
        //private string _count = string.Empty;
        //private string _equal = string.Empty;
        private int? _skip = null;
        private int? _take = null;
        private string _whereClause = string.Empty;

        //public string Any
        //{
        //    get
        //    {
        //        return _any;
        //    }
        //}

        public string Contains
        {
            get
            {
                return _contains;
            }
        }

        //public string Count
        //{
        //    get
        //    {
        //        return _count;
        //    }
        //}

        //public string Equal
        //{
        //    get
        //    {
        //        return _equal;
        //    }
        //}

        public int? Skip
        {
            get
            {
                return _skip;
            }
        }

        public int? Take
        {
            get
            {
                return _take;
            }
        }

        public string OrderBy
        {
            get
            {
                return _orderBy;
            }
        }

        public string WhereClause
        {
            get
            {
                return _whereClause;
            }
        }

        public QueryUtils()
        {
        }

        public string CompileQueryClause()
        {
            string query = "";

            if (!string.IsNullOrWhiteSpace(_contains))
            {
                query += _contains;
            }

            if (!string.IsNullOrWhiteSpace(_orderBy))
            {
                query += _orderBy;
            }

            if (!string.IsNullOrWhiteSpace(sb.ToString()) && string.IsNullOrWhiteSpace(query))
            {
                query = sb.ToString();
            }

            return query;
        }

        public string Translate(Expression expression)
        {
            this.sb = new StringBuilder();
            this.Visit(expression);
            return CompileQueryClause();
        }

        private static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType == typeof(Queryable) && m.Method.Name == "Where")
            {
                this.Visit(m.Arguments[0]);
                LambdaExpression lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
                this.Visit(lambda.Body);
                return m;
            }
            else if (m.Method.Name == "Take")
            {
                if (this.ParseTakeExpression(m))
                {
                    Expression nextExpression = m.Arguments[0];
                    return this.Visit(nextExpression);
                }
            }
            else if (m.Method.Name == "Skip")
            {
                if (this.ParseSkipExpression(m))
                {
                    Expression nextExpression = m.Arguments[0];
                    return this.Visit(nextExpression);
                }
            }
            else if (m.Method.Name == "OrderBy")
            {
                if (this.ParseOrderByExpression(m, "ASC"))
                {
                    Expression nextExpression = m.Arguments[0];
                    return this.Visit(nextExpression);
                }
            }
            else if (m.Method.Name == "OrderByDescending")
            {
                if (this.ParseOrderByExpression(m, "DESC"))
                {
                    Expression nextExpression = m.Arguments[0];
                    return this.Visit(nextExpression);
                }
            }
            else if (m.Method.Name == "Contains")
            {
                if (this.ParseContainsExpression(m))
                {
                    Expression nextExpression = m.Arguments[1];
                    return this.Visit(nextExpression);
                }
            }
            //else if (m.Method.Name == "Equals")
            //{
            //    if (this.ParseEqualsExpression(m))
            //    {
            //        Expression nextExpression = m.Arguments[1];
            //        return this.Visit(nextExpression);
            //    }
            //}
            //else if (m.Method.Name == "Any")
            //{
            //    if (this.ParseAnyExpression(m))
            //    {
            //        Expression nextExpression = m.Arguments[1];
            //        return this.Visit(nextExpression);
            //    }
            //}
            //else if (m.Method.Name == "Count")
            //{
            //    if (this.ParseCountExpression(m))
            //    {
            //        Expression nextExpression = m.Arguments[1];
            //        return this.Visit(nextExpression);
            //    }
            //}

            throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    sb.Append(" NOT ");
                    this.Visit(u.Operand);
                    break;
                case ExpressionType.Convert:
                    this.Visit(u.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
            }
            return u;
        }

        /// <summary>
        /// Visit Binary
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression b)
        {
            sb.Append("(");
            this.Visit(b.Left);

            switch (b.NodeType)
            {
                case ExpressionType.And:
                    sb.Append(" AND ");
                    break;

                case ExpressionType.AndAlso:
                    sb.Append(" AND ");
                    break;

                case ExpressionType.Or:
                    sb.Append(" OR ");
                    break;

                case ExpressionType.OrElse:
                    sb.Append(" OR ");
                    break;

                case ExpressionType.Equal:
                    if (IsNullConstant(b.Right))
                    {
                        sb.Append(" IS ");
                    }
                    else
                    {
                        sb.Append(" = ");
                    }
                    break;

                case ExpressionType.NotEqual:
                    if (IsNullConstant(b.Right))
                    {
                        sb.Append(" IS NOT ");
                    }
                    else
                    {
                        sb.Append(" <> ");
                    }
                    break;

                case ExpressionType.LessThan:
                    sb.Append(" < ");
                    break;

                case ExpressionType.LessThanOrEqual:
                    sb.Append(" <= ");
                    break;

                case ExpressionType.GreaterThan:
                    sb.Append(" > ");
                    break;

                case ExpressionType.GreaterThanOrEqual:
                    sb.Append(" >= ");
                    break;

                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));

            }

            this.Visit(b.Right);
            sb.Append(")");
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            IQueryable q = c.Value as IQueryable;

            if (q == null && c.Value == null)
            {
                sb.Append("NULL");
            }
            else if (q == null)
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        sb.Append(((bool)c.Value) ? 1 : 0);
                        break;

                    case TypeCode.String:
                        sb.Append("'");
                        sb.Append(c.Value);
                        sb.Append("'");
                        break;

                    case TypeCode.DateTime:
                        sb.Append("'");
                        sb.Append(c.Value);
                        sb.Append("'");
                        break;

                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));

                    default:
                        sb.Append(c.Value);
                        break;
                }
            }

            return c;
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
            {
                sb.Append(m.Member.Name);
                return m;
            }

            throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));
        }

        protected bool IsNullConstant(Expression exp)
        {
            return (exp.NodeType == ExpressionType.Constant && ((ConstantExpression)exp).Value == null);
        }

        //private bool ParseAnyExpression(MethodCallExpression expression)
        //{
        //    try
        //    {
        //        //TODO: Need to implement
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        //private bool ParseEqualsExpression(MethodCallExpression expression)
        //{
        //    try
        //    {
        //        //TODO: Need to implement
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        private bool ParseContainsExpression(MethodCallExpression expression)
        {
            try
            {
                var list = expression.Object;
                Expression operand;
                if (list == null)
                {
                    // Static method
                    // Must be Enumerable.Contains(source, item)
                    if (expression.Method.DeclaringType != typeof(Enumerable) || expression.Arguments.Count != 2) return false;
                    list = expression.Arguments[0];
                    operand = expression.Arguments[1];
                }
                else
                {
                    // Instance method
                    // Exclude string.Contains
                    if (list.Type == typeof(string)) return false;
                    // Must have a single argument
                    if (expression.Arguments.Count != 1) return false;
                    operand = expression.Arguments[0];
                    // The list must be IEnumerable<operand.Type>
                    if (!typeof(IEnumerable<>).MakeGenericType(operand.Type).IsAssignableFrom(list.Type)) return false;
                }
                // Try getting the list items
                object listValue;
                if (list.NodeType == ExpressionType.Constant)
                    // from constant value
                    listValue = ((ConstantExpression)list).Value;
                else
                {
                    // from constant value property/field
                    var listMember = list as MemberExpression;
                    if (listMember == null) return false;
                    var listOwner = listMember.Expression as ConstantExpression;
                    if (listOwner == null) return false;
                    var listProperty = listMember.Member as PropertyInfo;
                    listValue = listProperty != null ? listProperty.GetValue(listOwner.Value) : ((FieldInfo)listMember.Member).GetValue(listOwner.Value);
                }
                var listItems = listValue as System.Collections.IEnumerable;
                if (listItems == null) return false;

                // Do whatever you like with listItems
                var column = expression.Arguments[1].ToString().Split('.')[1];
                foreach (var item in listItems)
                {
                    _contains = (string.IsNullOrWhiteSpace(_contains) ? "(" : "AND ") + string.Format("{0} <> {1}", column, item);
                }
                _contains += ")";
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        //private bool ParseCountExpression(MethodCallExpression expression)
        //{
        //    try
        //    {
        //        //TODO: Need to implement
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        private bool ParseOrderByExpression(MethodCallExpression expression, string order)
        {
            UnaryExpression unary = (UnaryExpression)expression.Arguments[1];
            LambdaExpression lambdaExpression = (LambdaExpression)unary.Operand;

            lambdaExpression = (LambdaExpression)QueryEvaluatorUtils.PartialEval(lambdaExpression);

            MemberExpression body = lambdaExpression.Body as MemberExpression;
            if (body != null)
            {
                if (string.IsNullOrEmpty(_orderBy))
                {
                    _orderBy = string.Format("{0} {1}", body.Member.Name, order);
                }
                else
                {
                    _orderBy = string.Format("{0}, {1} {2}", _orderBy, body.Member.Name, order);
                }

                return true;
            }

            return false;
        }

        private bool ParseTakeExpression(MethodCallExpression expression)
        {
            ConstantExpression sizeExpression = (ConstantExpression)expression.Arguments[1];

            int size;
            if (int.TryParse(sizeExpression.Value.ToString(), out size))
            {
                _take = size;
                return true;
            }

            return false;
        }

        private bool ParseSkipExpression(MethodCallExpression expression)
        {
            ConstantExpression sizeExpression = (ConstantExpression)expression.Arguments[1];

            int size;
            if (int.TryParse(sizeExpression.Value.ToString(), out size))
            {
                _skip = size;
                return true;
            }

            return false;
        }
    }
}

