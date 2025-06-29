using KontrolWork1.Data;
using KontrolWork1.Domain;

namespace KontrolWork1.Managers;

public class OperationManager
{
    private readonly IRepository<Operation> _operationRepository;
    private readonly IDomainFactory _factory;

    public OperationManager(IRepository<Operation> operationRepository, IDomainFactory factory)
    {
        _operationRepository = operationRepository;
        _factory = factory;
    }

    public Operation CreateOperation(TransactionType type, BankAccount account, decimal amount, DateTime date, Category category, string description = null)
    {
        var operation = _factory.CreateOperation(type, account, amount, date, category, description);
        _operationRepository.Add(operation);
        // Обновляем баланс счета
        if (type == TransactionType.Income)
            account.Balance += amount;
        else
            account.Balance -= amount;
        return operation;
    }

    public void DeleteOperation(Guid operationId)
    {
        _operationRepository.Remove(operationId);
    }

    public IEnumerable<Operation> GetAllOperations()
    {
        return _operationRepository.GetAll();
    }
}