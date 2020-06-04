using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Lambda.Extension.Helper
{
    internal class WhereExpressionVisitor : ExpressionVisitor
    {

        private StringBuilder _express = new StringBuilder();
        private List<SqlParameter> _parameters = new List<SqlParameter>();
        public SqlParameter[] SqlParameters => _parameters.ToArray();

        public string Result { get { return _express.ToString(); } }

        #region 执行解析
        public void Resolve<T>(Expression<Func<T, bool>> expression)
        {
            if (expression != null)
            {
                if (_express.Length == 0)
                {
                    _express.Append("WHERE ");
                }
                Visit(expression.Body);
            }
        }
        #endregion

        #region 访问二元表达式
        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.OrElse || node.NodeType == ExpressionType.AndAlso)
                _express.Append("(");
            Visit(node.Left);
            _express.Append($" {node.NodeType.ConvertToOperator(node.Right.ToString() == "null")} ");
            Visit(node.Right);
            if (node.NodeType == ExpressionType.OrElse || node.NodeType == ExpressionType.AndAlso)
                _express.Append(")");
            return node;
        }
        #endregion

        #region 访问常量表达式
        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Value == null)
            {
                _express.Append("NULL");
            }
            else
            {
                _express.AppendFormat("{0}", ObjectToParameter(node.Value));
            }
            return node;
        }
        #endregion

        #region 访问成员表达式
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression is ParameterExpression)
            {
                _express.Append(node.Member.GetColumnAttributeName());
            }
            else
            {
                object val = null;
                var field = node.Member as FieldInfo;
                if (field != null)
                {
                    val = field.GetValue(((ConstantExpression)node.Expression).Value);
                }
                else
                {
                    val = ((PropertyInfo)node.Member).GetValue(((ConstantExpression)node.Expression).Value, null);
                }
                IEnumerator arr = val as IEnumerator;
                if (arr != null)
                {
                    _express.Append(ObjectToParameter(arr));
                }
                else if (val is IEnumerable)
                {
                    _express.Append(ObjectToParameter(((IEnumerable)val).GetEnumerator()));
                }
                else
                {
                    _express.Append(node.Member.GetColumnAttributeName());
                }
            }
            return node;
        }
        #endregion

        #region 访问方法表达式
        protected override System.Linq.Expressions.Expression VisitMethodCall(MethodCallExpression node)
        {
            Visit(node.Object);

            switch (node.Method.Name.ToLower())
            {
                case "equals":
                    this.Equals(node);
                    break;
                case "contains":
                    this.Contains(node);
                    break;
                case "startswith":
                    this.StartsWith(node);
                    break;
                case "endswith":
                    this.EndsWith(node);
                    break;
                default:
                    throw new Exception($"the expression is no support this function {node.Method.Name}");
            }

            return node;
        }

        #endregion

        #region 方法表达式

        private void Equals(MethodCallExpression node)
        {
            var argumentExpression = (ConstantExpression)node.Arguments[0];
            _express.AppendFormat(" ={0}", ObjectToParameter(argumentExpression.Value));
        }
        private void Contains(MethodCallExpression node)
        {
            if (node.Arguments.Count == 1)
            {
                var argumentExpression = (ConstantExpression)node.Arguments[0];
                if (argumentExpression.Value.GetType() == typeof(string))
                {
                    _express.AppendFormat(" LIKE {0}", ObjectToParameter('%' + argumentExpression.Value.ToString() + '%'));
                }
            }
            else if (node.Arguments.Count == 2)
            {
                var memberValue = (MemberExpression)node.Arguments[0];
                var memberKey = (MemberExpression)node.Arguments[1];
                _express.AppendFormat(" {0} IN (", memberKey.Member.GetColumnAttributeName());
                Visit(memberValue);
                _express.Append(')');
            }
        }
        private void StartsWith(MethodCallExpression node)
        {
            var argumentExpression = (ConstantExpression)node.Arguments[0];
            _express.AppendFormat(" LIKE {0}", ObjectToParameter('%' + argumentExpression.Value.ToString()));
        }
        private void EndsWith(MethodCallExpression node)
        {
            var argumentExpression = (ConstantExpression)node.Arguments[0];
            _express.AppendFormat(" LIKE {0}", ObjectToParameter(argumentExpression.Value.ToString() + '%'));
        }

        #endregion

        #region 辅助方法
        private object ObjectToParameter(IEnumerator arr)
        {
            if (arr.MoveNext())
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(ObjectToParameter(arr.Current));
                while (arr.MoveNext())
                {
                    builder.Append(',');
                    builder.Append(ObjectToParameter(arr.Current));
                }
                return builder.ToString();
            }
            else
            {
                return " IS NULL";
            }
        }

        private string ObjectToParameter(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return "NULL";
            }
            else
            {
                string key = 'p' + _parameters.Count.ToString();
                _parameters.Add(new SqlParameter(key, obj));
                return '@' + key;
            }
        }

        #endregion

    }

    internal class ParameterRebinder : ExpressionVisitor
    {
        readonly Dictionary<ParameterExpression, ParameterExpression> _parameterMap;

        ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            _parameterMap = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map,
            Expression newParameters)
        {
            return new ParameterRebinder(map).Visit(newParameters);
        }

        protected override Expression VisitParameter(ParameterExpression newParameters)
        {
            if (_parameterMap.TryGetValue(newParameters, out var replacement))
            {
                newParameters = replacement;
            }

            return base.VisitParameter(newParameters);
        }
    }

}
