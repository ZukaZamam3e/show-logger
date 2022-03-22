using System.Linq.Expressions;
using System.Reflection;

namespace ShowLogger.Web.Utilities;

public static class ExpressionExtensions
{
    public static Type GetObjectType<T>(this Expression<Func<T, object>> expr)
    {
        if ((expr.Body.NodeType == ExpressionType.Convert) ||
            (expr.Body.NodeType == ExpressionType.ConvertChecked))
        {
            var unary = expr.Body as UnaryExpression;
            if (unary != null)
                return unary.Operand.Type;
        }
        return expr.Body.Type;
    }

    public static PropertyInfo GetPropertyInfo<TSource, TProperty>(this Expression<Func<TSource, TProperty>> propertyLambda, TSource source)
    {
        Type type = typeof(TSource);

        Expression body = propertyLambda.Body;

        if (propertyLambda.Body.NodeType == ExpressionType.Convert)
            body = ((UnaryExpression)body).Operand;

        MemberExpression member = body as MemberExpression;
        if (member == null)
            throw new ArgumentException(string.Format(
                "Expression '{0}' refers to a method, not a property.",
                propertyLambda.ToString()));

        PropertyInfo propInfo = member.Member as PropertyInfo;
        if (propInfo == null)
            throw new ArgumentException(string.Format(
                "Expression '{0}' refers to a field, not a property.",
                propertyLambda.ToString()));

        if (type != propInfo.ReflectedType &&
            !type.IsSubclassOf(propInfo.ReflectedType))
            throw new ArgumentException(string.Format(
                "Expression '{0}' refers to a property that is not from type {1}.",
                propertyLambda.ToString(),
                type));

        return propInfo;
    }
}
