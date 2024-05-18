using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Infrastructure.Persistence.Repositories;

internal class BaseRepository<T, TId> 
    : IAsyncRepository<T, TId> where T : class, Domain.Common.IEntity<TId>
{
    private readonly ScrumboardDbContext _dbContext;
    private readonly ISpecificationEvaluator _specificationEvaluator;

    public BaseRepository(ScrumboardDbContext dbContext)
    {
        _dbContext = dbContext;
        _specificationEvaluator = SpecificationEvaluator.Default;
    }

    public virtual async Task<T> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        var keyValues = new object[] { id };
        
        return await _dbContext.Set<T>().FindAsync(keyValues, cancellationToken);
    }

    public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default) 
        => await _dbContext.Set<T>().ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<T>> ListAsync(
        ISpecification<T> spec, 
        CancellationToken cancellationToken = default)
    {
        var specificationResult = ApplySpecification(spec);
        
        return await specificationResult.ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(
        ISpecification<T> spec, 
        CancellationToken cancellationToken = default)
    {
        var specificationResult = ApplySpecification(spec);
        
        return await specificationResult.CountAsync(cancellationToken);
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Add(entity);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Remove(entity);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<T> FirstAsync(
        ISpecification<T> spec, 
        CancellationToken cancellationToken = default)
    {
        var specificationResult = ApplySpecification(spec);
        
        return await specificationResult.FirstAsync(cancellationToken);
    }

    public async Task<T> FirstOrDefaultAsync(
        ISpecification<T> spec, 
        CancellationToken cancellationToken = default)
    {
        var specificationResult = ApplySpecification(spec);
        
        return await specificationResult.FirstOrDefaultAsync(cancellationToken);
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec) 
        => _specificationEvaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
}