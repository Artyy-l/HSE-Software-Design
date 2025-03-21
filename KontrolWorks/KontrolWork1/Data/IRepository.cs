namespace KontrolWork1.Data;

public interface IRepository<T>
{
    void Add(T item);
    void Remove(Guid id);
    T GetById(Guid id);
    IEnumerable<T> GetAll();
}
