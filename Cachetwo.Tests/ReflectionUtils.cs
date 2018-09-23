using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cachetwo
{
    internal static class ReflectionUtils
    {
        public static MethodInfo GetMethodInfo(Expression<Action> expression)
            => GetMethodInfo(expression as LambdaExpression);

        public static MethodInfo GetMethodInfo(LambdaExpression expression)
        {
            if (expression?.Body is MethodCallExpression methodCallExpression)
            {
                return methodCallExpression.Method;
            }
            throw new ArgumentException("Invalid method call expression");
        }
    }
}
