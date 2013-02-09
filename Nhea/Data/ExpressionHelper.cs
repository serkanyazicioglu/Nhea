using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace Nhea.Data
{
    public static class ExpressionHelper
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            if (first == null)
            {
                return second;
            }
            else if (second == null)
            {
                return first;
            }

            Replace(second, second.Parameters[0], first.Parameters[0]);
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(first.Body, second.Body), first.Parameters);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            if (first == null)
            {
                return second;
            }
            else if (second == null)
            {
                return first;
            }

            Replace(second, second.Parameters[0], first.Parameters[0]);
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(first.Body, second.Body), first.Parameters);
        }

        static void Replace(object instance, object old, object replacement)
        {
            for (Type t = instance.GetType(); t != null; t = t.BaseType)
                foreach (FieldInfo fi in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    object val = fi.GetValue(instance);
                    if (val != null && val.GetType().Assembly == typeof(Expression).Assembly)
                        if (object.ReferenceEquals(val, old))
                            fi.SetValue(instance, replacement);
                        else
                            Replace(val, old, replacement);
                }
        }
    }
}