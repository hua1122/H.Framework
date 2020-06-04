using System.Linq.Expressions;

namespace Lambda.Extension.Helper
{
    public static class OperateHelper
    {
        //操作符转换
        internal static string ConvertToOperator(this ExpressionType type, bool useIs = false)
        {
            string operand = string.Empty;
            switch (type)
            {
                case ExpressionType.AndAlso:
                    operand = "AND";
                    break;
                case ExpressionType.OrElse:
                    operand = "OR";
                    break;
                case ExpressionType.Equal:
                    if (useIs)
                        operand = "IS";
                    else
                        operand = "=";
                    break;
                case ExpressionType.NotEqual:
                    if (useIs)
                        operand = "IS NOT";
                    else
                        operand = "<>";
                    break;
                case ExpressionType.LessThan:
                    operand = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    operand = "<=";
                    break;
                case ExpressionType.GreaterThan:
                    operand = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    operand = ">=";
                    break;
                default:
                    throw new System.NotImplementedException($"无法解析节点{type}");
            }
            return operand;
        }

    }
}
