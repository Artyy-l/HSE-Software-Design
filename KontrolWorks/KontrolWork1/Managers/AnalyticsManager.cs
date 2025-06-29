using KontrolWork1.Data;
using KontrolWork1.Domain;

namespace KontrolWork1.Managers;

public class AnalyticsManager
{
    private readonly IRepository<Operation> _operationRepository;

    public AnalyticsManager(IRepository<Operation> operationRepository)
    {
        _operationRepository = operationRepository;
    }
    
    public decimal CalculateBalanceDifference(DateTime start, DateTime end)
    {
        decimal income = 0, expense = 0;
        foreach (var op in _operationRepository.GetAll())
        {
            if (op.Date >= start && op.Date <= end)
            {
                if (op.Type == TransactionType.Income)
                    income += op.Amount;
                else
                    expense += op.Amount;
            }
        }
        return income - expense;
    }
    
    public Dictionary<Guid, decimal> GroupOperationsByCategory()
    {
        var groups = new Dictionary<Guid, decimal>();
        foreach (var op in _operationRepository.GetAll())
        {
            if (groups.ContainsKey(op.CategoryId))
                groups[op.CategoryId] += op.Amount;
            else
                groups[op.CategoryId] = op.Amount;
        }
        return groups;
    }
}