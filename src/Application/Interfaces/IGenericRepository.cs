using System.Linq.Expressions;

namespace Application.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);

    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    void Remove(T entity);

    Task<(IEnumerable<T> Items, int TotalCount)>
    GetPagedAsync(int pageNumber, int pageSize);

}