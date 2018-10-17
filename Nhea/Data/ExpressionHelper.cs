using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Nhea.Data
{
    public static class ExpressionHelper
    {
        //public static Expression<Func<T>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        //{
        //    if (first == null)
        //    {
        //        return second;
        //    }
        //    else if (second == null)
        //    {
        //        return first;
        //    }

        //    //Replace(second, second.Parameters[0], first.Parameters[0]);
        //    var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
        //    var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

        //    return Expression.Lambda<Func<T>>(Expression.And(first.Body, secondBody), first.Parameters);
        //}

        //public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        //{
            

        //    //Replace(second, second.Parameters[0], first.Parameters[0]);
        //    var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
        //    var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

        //    return Expression.Lambda<Func<T, bool>>(Expression.Or(first.Body, secondBody), first.Parameters);
        //}

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

            return first.Compose(second, Expression.And);
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

            return first.Compose(second, Expression.Or);
        }

        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        //static void Replace(object instance, object old, object replacement)
        //{
        //    for (Type t = instance.GetType(); t != null; t = t.BaseType)
        //        foreach (FieldInfo fi in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        //        {
        //            object val = fi.GetValue(instance);
        //            if (val != null && val.GetType().Assembly == typeof(Expression).Assembly)
        //                if (object.ReferenceEquals(val, old))
        //                    fi.SetValue(instance, replacement);
        //                else
        //                    Replace(val, old, replacement);
        //        }
        //}
    }

    internal class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;

            if (map.TryGetValue(p, out replacement))
            {
                p = replacement;

            }

            return base.VisitParameter(p);
        }
    }
}