using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Lambda.Extension.Helper
{
    public static class AttributeHelper
    {
        public static string GetTableAttributeName(this Type type)
        {
            return type.GetCustomAttribute<TableAttribute>()?.Name ?? type.Name;
        }
        public static string GetColumnAttributeName(this MemberInfo memberInfo)
        {
            return memberInfo.GetCustomAttribute<ColumnAttribute>()?.Name ?? memberInfo.Name;
        }

    }
}
