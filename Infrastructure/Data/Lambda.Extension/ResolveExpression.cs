using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Lambda.Extension.Helper;

namespace Lambda.Extension
{
    public class ResolveExpression
    {
        public static string ResolveSelect<T>(PropertyInfo[] properties=null, uint? topNum=null)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT ");

            if (topNum.HasValue)
            {
                sqlBuilder.AppendFormat("TOP {0} ", topNum.Value);
            }
            if (properties==null)
            {
                properties = typeof(T).GetProperties();
            }
            if (properties != null)
            {
                var propertyBuilder = new StringBuilder();
                foreach (var property in properties)
                {
                    NotMappedAttribute notMappedAttribute = property.GetCustomAttribute(typeof(NotMappedAttribute)) as NotMappedAttribute;
                    if (notMappedAttribute == null)
                    {
                        if (propertyBuilder.Length > 0)
                        {
                            propertyBuilder.Append(",");
                        }
                        propertyBuilder.AppendFormat("{0} AS '{1}'", property.GetColumnAttributeName(), property.Name);
                    }
                }
                sqlBuilder.Append(propertyBuilder);
            }
            return sqlBuilder.ToString();
        }

        public static string ResolveCount<T>()
        {
            string sql = "SELECT Count(1) " + ResolveFrom<T>();
            return sql;
        }

        public static string ResolveCount<T>(Expression<Func<T, bool>> expression)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT Count(1) {0} ", ResolveFrom<T>());
            sqlBuilder.Append(ResolveWhere<T>(expression));

            return sqlBuilder.ToString();
        }

        public static string ResolveFrom<T>()
        {
            return "FROM " + typeof(T).GetTableAttributeName();
        }

        public static string ResolveWhere<T>(Expression<Func<T, bool>> expression)
        {
            //解析表达式
            var where = new WhereExpressionVisitor();
            where.Resolve(expression);
            return where.Result;
        }

        public static string ResolveOrderBy<T>(Expression<Func<T, object>> expression, bool isDesc = false)
        {
            string propertys = null;
            if (expression.Body is MemberExpression)
            {
                propertys = ((MemberExpression)expression.Body).Member.GetColumnAttributeName();
            }
            else if (expression.Body is UnaryExpression)
            {
                propertys = ((MemberExpression)((UnaryExpression)expression.Body).Operand).Member.GetColumnAttributeName();
            }
            else if (expression.Body is ParameterExpression)
            {
                propertys = ((ParameterExpression)expression.Body).Type.GetColumnAttributeName();
            }
            else if (expression.Body is NewExpression)
            {
                propertys = string.Join(",", ((NewExpression)expression.Body).Members.Select(x => x.GetColumnAttributeName()).ToArray());
            }

            return "ORDER BY " + string.Join(",", propertys) + (isDesc ? " DESC" : " ASC ");
        }

        public static string ResolveGroupBy<T>(Expression<Func<T, object>> expression)
        {
            string propertys = null;
            if (expression.Body is MemberExpression)
            {
                propertys = ((MemberExpression)expression.Body).Member.GetColumnAttributeName();
            }
            else if (expression.Body is UnaryExpression)
            {
                propertys = ((MemberExpression)((UnaryExpression)expression.Body).Operand).Member.GetColumnAttributeName();
            }
            else if (expression.Body is ParameterExpression)
            {
                propertys = ((ParameterExpression)expression.Body).Type.GetColumnAttributeName();
            }
            else if (expression.Body is NewExpression)
            {
                propertys = string.Join(",", ((NewExpression)expression.Body).Members.Select(x => x.GetColumnAttributeName()).ToArray());
            }

            return "GROUP BY " + string.Join(",", propertys);
        }



    }


}
