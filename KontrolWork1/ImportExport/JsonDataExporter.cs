using System.Text.Json;
using KontrolWork1.Domain;

namespace KontrolWork1.ImportExport;

public class JsonDataExporter : IDataExporterVisitor
{
    public void Export(IEnumerable<BankAccount> accounts, IEnumerable<Category> categories, IEnumerable<Operation> operations)
    {
        Console.WriteLine("Экспорт данных в JSON формате:");
        foreach (var acc in accounts)
        {
            acc.Accept(this);
        }
        foreach (var cat in categories)
        {
            cat.Accept(this);
        }
        foreach (var op in operations)
        {
            op.Accept(this);
        }
    }

    public void Visit(BankAccount account)
    {
        Console.WriteLine($"Экспорт в файл JSON (BankAccount): {account.Id}, {account.Name}, {account.Balance}");
    }

    public void Visit(Category category)
    {
        Console.WriteLine($"Экспорт в файл JSON (Category): {category.Id}, {category.Name}, {category.Type}");
    }

    public void Visit(Operation operation)
    {
        Console.WriteLine($"Экспорт в файл JSON (Operation): {operation.Id}, {operation.Type}, {operation.Amount}, {operation.Date}, {operation.Description}");
    }
}