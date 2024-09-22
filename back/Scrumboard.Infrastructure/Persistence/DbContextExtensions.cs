using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Scrumboard.Infrastructure.Persistence;

internal static class DbContextExtensions
{
    public static async Task LoadNavigationPropertyAsync<TEntity, TProperty>(
        this DbContext dbContext,
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> collectionSelector,
        CancellationToken cancellationToken = default) 
        where TEntity : class
        where TProperty : class
    {
        var entry = dbContext.Entry(entity);
        
        var navigation = entry.Navigations
            .FirstOrDefault(x => x.Metadata.Name == GetPropertyName(collectionSelector));

        if (navigation is { IsLoaded: false })
        {
            await navigation.LoadAsync(cancellationToken);
        }
    }

    private static string GetPropertyName<TEntity, TProperty>(
        Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression)
    {
        if (propertyExpression.Body is MemberExpression member)
        {
            return member.Member.Name;
        }

        throw new ArgumentException("Cannot get property name: Invalid expression", nameof(propertyExpression));
    }
}
