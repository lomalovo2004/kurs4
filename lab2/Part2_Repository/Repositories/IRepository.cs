namespace Part2_Repository.Repositories;

public interface IRepository<T> where T : class
{
    T? GetById(int id);
    IReadOnlyList<T> GetAll();
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}
