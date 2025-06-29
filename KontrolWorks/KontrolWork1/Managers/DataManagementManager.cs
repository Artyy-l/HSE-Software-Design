using KontrolWork1.Data;
using KontrolWork1.Domain;

namespace KontrolWork1.Managers;

public class DataManagementManager
{
    private readonly IRepository<BankAccount> _accountRepository;
    private readonly IRepository<Operation> _operationRepository;

    public DataManagementManager(IRepository<BankAccount> accountRepository, IRepository<Operation> operationRepository)
    {
        _accountRepository = accountRepository;
        _operationRepository = operationRepository;
    }

    public void RecalculateBalance(BankAccount account)
    {
        decimal recalculatedBalance = 0;
        var operations = _operationRepository.GetAll().Where(op => op.BankAccountId == account.Id);
        foreach (var op in operations)
        {
            if (op.Type == TransactionType.Income)
                recalculatedBalance += op.Amount;
            else
                recalculatedBalance -= op.Amount;
        }
        account.Balance = recalculatedBalance;
    }
    public void RecalculateAllAccounts()
    {
        foreach (var account in _accountRepository.GetAll())
        {
            RecalculateBalance(account);
        }
    }
}