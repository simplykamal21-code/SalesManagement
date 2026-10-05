using System.Linq.Expressions;

namespace SMS.API.Repositories.Interfaces;

// Bare-bones generic repository so each entity repo isn't rewriting the same CRUD boilerplate.
public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(params object[] keyValues);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task<int> SaveChangesAsync();
}
