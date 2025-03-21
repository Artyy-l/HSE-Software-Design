namespace KontrolWork1.Data;

public class InMemoryRepository<T> : IRepository<T> where T : class
{
    private readonly Dictionary<Guid, T> _store = new Dictionary<Guid, T>();
    
    public void Add(T item)
    {
        var id = (Guid)item.GetType().GetProperty("Id").GetValue(item);
        _store[id] = item;
    }

    public void Remove(Guid id)
    {
        _store.Remove(id);
    }

    public T GetById(Guid id)
    {
        _store.TryGetValue(id, out var item);
        return item;
    }

    public IEnumerable<T> GetAll()
    {
        return _store.Values;
    }
}