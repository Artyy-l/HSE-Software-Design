using KontrolWork1.Data;
using KontrolWork1.Domain;
using KontrolWork1.Managers;
using KontrolWork1.UI;

namespace KontrolWork1;

public class Program
{
    public static void Main(string[] args)
    {
        // на всякий случай
        try
        {
            IDomainFactory factory = new DomainFactory();

            var accountRepo = new InMemoryRepository<BankAccount>();
            var categoryRepo = new InMemoryRepository<Category>();
            var operationRepo = new InMemoryRepository<Operation>();

            var accountManager = new BankAccountManager(accountRepo, factory);
            var categoryManager = new CategoryManager(categoryRepo, factory);
            var operationManager = new OperationManager(operationRepo, factory);
            var analyticsManager = new AnalyticsManager(operationRepo);
            var dataManagementManager = new DataManagementManager(accountRepo, operationRepo);

            var ui = new ConsoleUI(accountManager, categoryManager, operationManager, analyticsManager,
                dataManagementManager, factory);
            ui.Run();

            Console.WriteLine("Приложение завершено. Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}