using System;
using System.Linq;
using System.Linq.Expressions;
using Lambda.Extension.Helper;

namespace Lambda.Extension
{
    public static class ExpressionBuilder
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            if (first != null && second != null)
            {
                return first.Compose(second, Expression.AndAlso);
            }
            else if (first != null)
            {
                return first;
            }
            else if (second != null)
            {
                return second;
            }
            else
            {
                return null;
            }

        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {


            if (first != null && second != null)
            {
                return first.Compose(second, Expression.OrElse);
            }
            else if (first != null)
            {
                return first;
            }
            else if (second != null)
            {
                return second;
            }
            else
            {
                return null;
            }
        }

        private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second,
            Func<Expression, Expression, Expression> merge
        )
        {
            var map = first.Parameters
                .Select((oldParam, index) => new { oldParam, newParam = second.Parameters[index] })
                .ToDictionary(p => p.newParam, p => p.oldParam);

            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }
    }



}
