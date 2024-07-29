namespace Scrumboard.SharedKernel.Extensions;

public static class ValueTaskGenericExtensions
{
    public static async ValueTask<TEntity> OrThrowEntityNotFoundAsync<TEntity>(this ValueTask<TEntity?> valueTaskEntity)
        where TEntity : class
    {
        var entity = await valueTaskEntity;

        ArgumentNullException.ThrowIfNull(entity);

        return entity;
    }
}
