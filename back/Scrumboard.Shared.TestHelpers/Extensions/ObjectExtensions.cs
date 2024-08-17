using System.Linq.Expressions;
using System.Reflection;

namespace Scrumboard.Shared.TestHelpers.Extensions;

public static class ObjectExtensions
{
    public static void SetProperty<T, TProperty>(
        this T obj, 
        Expression<Func<T, TProperty>> propertyExpression, 
        TProperty value) where T : class
    {
        if (propertyExpression.Body is not MemberExpression memberExpression)
        {
            throw new ArgumentException("Expression must be a property access.", nameof(propertyExpression));
        }

        if (memberExpression.Member is not PropertyInfo property)
        {
            throw new ArgumentException("Expression must be a property access.", nameof(propertyExpression));
        }

        if (!property.CanWrite && property.GetSetMethod(true) is null)
        {
            throw new InvalidOperationException($"Property '{property.Name}' cannot be set.");
        }
        
        property.SetValue(obj, value);
    }
}
