using System.Linq.Expressions;
using Portfolio.Domain.Entities.Base;

namespace Portfolio.Application.Abstraction.Repositories;

public interface IGenericRepository<T> where T : BaseEntity, new()
{
    Task<IEnumerable<T>> GetAllAsync(bool asNoTrack = true, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,  params string[]? includes);
    
    Task<T?> GetByIdAsync(Guid id, bool asNoTrack = true,  params string[]? includes);
    Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<Guid> ids, bool asNoTrack = true,  params string[]? includes);
    
    Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate, bool asNoTrack = true,  params string[]? includes);
    Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate, bool asNoTrack = true,  params string[]? includes);
    
    Task<bool> IsExistAsync(Expression<Func<T, bool>> expression);
    Task<bool> IsExistRangeAsync(Guid[] ids);
    Task<bool> IsExistAsync(Guid id); 

    Task<IEnumerable<T>> GetPagedAsync(Expression<Func<T, bool>> predicate, int pageNumber = 1, int pageSize = 12, bool asNoTrack = true,Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,  params string[]? includes);
    
    Task AddAsync(T  entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    
    Task UpdateAsync(T entity);
    
    Task HardDeleteAsync(Guid id);
    Task SoftDeleteAsync(Guid id);
    Task ReverseDeleteAsync(Guid id);
    // Delete Range Async 
    
    Task<int>  HardDeleteRangeAsync(Guid[] ids);
    Task<int> SoftDeleteRangeAsync(Guid[] ids); 
    Task<int> ReverseDeleteRangeAsync(Guid[] ids);
    
    Task<int> DeleteAndSaveAsync(Guid id);
    Task<int> SaveAsync(); 
}