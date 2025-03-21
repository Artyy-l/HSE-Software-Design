using KontrolWork1.Data;
using KontrolWork1.Domain;

namespace KontrolWork1.Managers;

public class CategoryManager
{
    private readonly IRepository<Category> _categoryRepository;
    private readonly IDomainFactory _factory;

    public CategoryManager(IRepository<Category> categoryRepository, IDomainFactory factory)
    {
        _categoryRepository = categoryRepository;
        _factory = factory;
    }

    public Category CreateCategory(TransactionType type, string name)
    {
        var category = _factory.CreateCategory(type, name);
        _categoryRepository.Add(category);
        return category;
    }

    public void DeleteCategory(Guid categoryId)
    {
        _categoryRepository.Remove(categoryId);
    }

    public IEnumerable<Category> GetAllCategories()
    {
        return _categoryRepository.GetAll();
    }
}