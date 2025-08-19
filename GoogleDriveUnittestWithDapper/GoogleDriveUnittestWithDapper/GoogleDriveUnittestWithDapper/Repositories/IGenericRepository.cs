using GoogleDriveUnittestWithDapper.Dto;
using System.Linq.Expressions;

namespace GoogleDriveUnittestWithDapper.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAllQueryable();
        Task<IQueryable<T>> GetAllQueryableAsync();
        Task<T> GetByIdAsync(int id);
        Task<int> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(int id);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> predicate);
        Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> predicate);

        // Custom method for BannedUser
        Task<IQueryable<BannedUserDto>> GetBannedUsersByUserIdAsync(int userId);
    }
}
