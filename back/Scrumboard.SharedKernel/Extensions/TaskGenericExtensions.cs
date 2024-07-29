using Scrumboard.SharedKernel.Exceptions;

namespace Scrumboard.SharedKernel.Extensions;

public static class TaskGenericExtensions
{
    public static async Task<TDomainEntity> OrThrowResourceNotFoundAsync<TDomainEntity, TStrongId>(
        this Task<TDomainEntity?> taskEntity,
        TStrongId id)
        where TDomainEntity : class
        where TStrongId : struct
    {
        var entity = await taskEntity;
        return entity ?? throw new NotFoundException(typeof(TDomainEntity).Name, id);
    }
}
