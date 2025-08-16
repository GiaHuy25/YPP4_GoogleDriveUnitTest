namespace MVCImplement.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
