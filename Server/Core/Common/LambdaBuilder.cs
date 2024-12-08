using Core.DTOs;
using Data.Entities.TestEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common
{
    public static class LambdaBuilder
    {
        public static Expression<Func<T, bool>> GetAndLambdaExpression<T>(List<ExpressionFilter> filters)
        {
            if (filters.Count == 0)
            {
                return null;
            }

            //tạo tham số đại diện cho T
            ParameterExpression t = Expression.Parameter(typeof(T), "t");
            //Biểu thức lambda trả về
            Expression exp = null;

            exp = GetExpression<T>(t, filters[0]);
            for (int i = 1; i < filters.Count; i++)
            {
                exp = Expression.Add(exp, GetExpression<T>(t, filters[i]));
            }

            return Expression.Lambda<Func<T, bool>>(exp, t);
        }


        public static Expression<Func<Test, bool>> GetTestSearchingExpression(TestSearchDTO criteria)
        {
            // element in lamda
            var e = Expression.Parameter(typeof(Test), "e");
            // expression in lamda
            var expressions = new List<Expression>();

            // Contains condition
            MethodInfo containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) })!;

            // Name and Description
            if (!string.IsNullOrEmpty(criteria.Name))
            {
                var nameProp = Expression.Property(e, nameof(criteria.Name));
                var valueProp = Expression.Constant(criteria.Name);
                expressions.Add(Expression.Call(nameProp, containsMethod, valueProp));
            }
            if (!string.IsNullOrEmpty(criteria.Description))
            {
                var nameProp = Expression.Property(e, nameof(criteria.Description));
                var valueProp = Expression.Constant(criteria.Description);
                expressions.Add(Expression.Call(nameProp, containsMethod, valueProp));
            }

            // Category
            if (!string.IsNullOrEmpty(criteria.CategorySlug))
            {
                var nameProp = Expression.Property(e, nameof(criteria.CategorySlug));
                var valueProp = Expression.Constant(criteria.CategorySlug);
                expressions.Add(Expression.Equal(nameProp, valueProp));
            }

            // Combine all expressions
            Expression? combinedExpressions = null;
            foreach (var expression in expressions)
            {
                combinedExpressions = combinedExpressions == null? 
                    expression : Expression.AndAlso(combinedExpressions, expression);
            }

            if(combinedExpressions == null)
            {
                return e => true;
            }
            return Expression.Lambda<Func<Test, bool>>(combinedExpressions, e);
        }

        public static Expression<Func<T, bool>> GetOrLambdaExpression<T>(List<ExpressionFilter> filters)
        {
            if (filters.Count == 0)
            {
                return null;
            }

            //tạo tham số đại diện cho T
            ParameterExpression t = Expression.Parameter(typeof(T), "t");
            //Biểu thức lambda trả về
            Expression exp = null;

            exp = GetExpression<T>(t, filters[0]);
            for (int i = 1; i < filters.Count; i++)
            {
                exp = Expression.Or(exp, GetExpression<T>(t, filters[i]));
            }

            return Expression.Lambda<Func<T, bool>>(exp, t);
        }




        //Gen expression for lambda
        private static Expression GetExpression<T>(ParameterExpression param, ExpressionFilter filter)
        {
            MethodInfo? containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            MethodInfo? startWithsMethod = typeof(string).GetMethod("StartWiths", new Type[] { typeof(string) });
            MethodInfo? endWithsMethod = typeof(string).GetMethod("EndWiths", new Type[] { typeof(string) });

            //param : t trong t => t.property
            //~ param.(filter.Property!)
            MemberExpression member = Expression.Property(param, filter.Property!);
            ConstantExpression constant = Expression.Constant(filter.Value);

            switch (filter.Comparison)
            {
                case Comparison.Equal:
                    return Expression.Equal(member, constant);
                case Comparison.LessThan:
                    return Expression.LessThan(member, constant);
                case Comparison.GreaterThan:
                    return Expression.GreaterThan(member, constant);
                case Comparison.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, constant);
                case Comparison.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);
                case Comparison.NotEqual:
                    return Expression.NotEqual(member, constant);
                case Comparison.Contains:
                    return Expression.Call(member, containsMethod!, constant);
                case Comparison.StartsWith:
                    return Expression.Call(member, startWithsMethod!, constant);
                case Comparison.EndsWith:
                    return Expression.Call(member, endWithsMethod!, constant);
                default:
                    return null;
            }    

        }    
    }
}
