using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstraction.Repositories;
using Portfolio.Domain.Entities.Base;
using Portfolio.Persistence.Contexts;

namespace Portfolio.Persistence.Implementations.Repositories;

public class GenericRepository<T>(PortfolioDbContext context) : IGenericRepository<T> where T: BaseEntity, new()
{
    protected DbSet<T> Table = context.Set<T>();

    public async Task<IEnumerable<T>> GetAllAsync(bool asNoTrack = true, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, params string[]? includes)
    {
        IQueryable<T> query = Table; 
        if(predicate != null)
            query = query.Where(predicate);
        
        if (orderBy != null)
            query = orderBy(query);

        return await _includeAndTracking(query, includes, asNoTrack).ToListAsync(); 
    }

    public async Task<T?> GetByIdAsync(Guid id, bool asNoTrack = true, params string[]? includes)
        => await _includeAndTracking(
                Table.Where(x => x.Id == id),
                includes,
                asNoTrack)
            .SingleOrDefaultAsync();
       

    public async Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<Guid> ids, bool asNoTrack = true, params string[]? includes)
    {
        if (!ids.Any())
            return Array.Empty<T>();
        
        IQueryable<T> query = Table.Where(x => ids.Contains(x.Id));
        query = _includeAndTracking(query, includes, asNoTrack);
        return await query.ToListAsync();
    }
    
    public async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> expression, bool asNoTrack = true, params string[]? includes)
        => await _includeAndTracking(Table.Where(expression), includes, asNoTrack).ToListAsync();   
    
    public async Task<T?> GetFirstAsync(Expression<Func<T, bool>> expression, bool asNoTrack = true, params string[]? includes)
        => await _includeAndTracking(Table.Where(expression), includes, asNoTrack).FirstOrDefaultAsync();   
    
    public async Task<bool> IsExistAsync(Expression<Func<T, bool>> expression)
        => await Table.AnyAsync(expression);
    
    // Is Exist Async WithId 
    public async Task<bool> IsExistAsync(Guid id)
        => await Table.AnyAsync(x => x.Id == id);

    public async Task<bool> IsExistRangeAsync(Guid[] ids)
    { 
        var count = await Table.CountAsync(x => ids.Contains(x.Id));
        return count == ids.Length;
    }

    public async Task<IEnumerable<T>> GetPagedAsync(Expression<Func<T, bool>> expression, int pageNumber = 1, int pageSize = 12, bool asNoTrack = true,Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,  params string[]? includes)
    {
        IQueryable<T> query = Table; 
        if(expression != null) 
            query = query.Where(expression);

        query = _includeAndTracking(query, includes, asNoTrack);
        
        if(orderBy != null)
            query = orderBy(query);
        
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return items; 
    }

    // Create 
    public async Task AddAsync(T entity)
        => await Table.AddAsync(entity);

    // Create Range 
    public async Task AddRangeAsync(IEnumerable<T> entities)
        => await Table.AddRangeAsync(entities);

    // Update
    public Task UpdateAsync(T entity)
    {
        Table.Update(entity);
        return Task.CompletedTask;
    }      

    // Hard Delete
    public async Task HardDeleteAsync(Guid id)
    {
        var entity = await Table.FindAsync(id);
        if (entity is not null)
            Table.Remove(entity);
        Table.Remove(entity);
    }
    
    // Soft Delete
    public async Task SoftDeleteAsync(Guid id)
    {
        var entity = await Table.FindAsync(id);
        if(!entity.IsDeleted && entity is not null)
            entity.IsDeleted = true;
    }
    
    // Reverse Delete
    public async Task ReverseDeleteAsync(Guid id)
    {
        var entity = await Table.FindAsync(id);
        if(entity.IsDeleted)
            entity.IsDeleted = false;
    }

    // Hard Delete Range 
    public async Task<int> HardDeleteRangeAsync(Guid[] ids)
    {
        var entities = await Table.Where(x => ids.Contains(x.Id)).ToListAsync();
        if (!entities.Any())
            return await Task.FromResult(0);

        Table.RemoveRange(entities);
        return await Task.FromResult(entities.Count);
    }

    // Soft Delete Range Async 
    public async Task<int> SoftDeleteRangeAsync(Guid[] ids)
        => await Table.Where(x => ids.Contains(x.Id))
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true));

    // Reverse Delete Range Async 
    public async Task<int> ReverseDeleteRangeAsync(Guid[] ids)
        => await Table.Where(x => ids.Contains(x.Id))
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, false));

    // Delete And Save Async 
    public async Task<int> DeleteAndSaveAsync(Guid id)
        => await Table.Where(x => x.Id == id).ExecuteDeleteAsync();

    // Save Async 
    public async Task<int> SaveAsync()
        => await context.SaveChangesAsync();     
    

    private IQueryable<T> _includeAndTracking(IQueryable<T> query, string[]? includes, bool asNoTrack)
    {
        if (includes is not null && includes.Length > 0)
        {
            query = _checkIncludes(query, includes);
            if (asNoTrack)
                query = query.AsNoTrackingWithIdentityResolution();
        }
        else
            if (asNoTrack)
                query = query.AsNoTracking();
        
        return query;
    }

    private IQueryable<T> _checkIncludes(IQueryable<T> query, string[]? includes)
    {
        foreach (var include in includes)
            query = query.Include(include);
        return query;
    }
}
