namespace Application.Contracts.Interfaces.Common
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Query(); // Optional, only if needed
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task AddRangeAsync(List<T> entities);
        void Update(T entity);
        void Delete(T entity);
    }

}
