using KontrolWork1.Managers;

namespace KontrolWork1.Commands;

public class RecalculateBalanceCommand : ICommand
{
    private readonly DataManagementManager _dataManagementManager;

    public RecalculateBalanceCommand(DataManagementManager dataManagementManager)
    {
        _dataManagementManager = dataManagementManager;
    }

    public void Execute()
    {
        _dataManagementManager.RecalculateAllAccounts();
        Console.WriteLine("Пересчет баланса завершен.");
    }
}