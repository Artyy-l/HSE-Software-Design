using System.Text;
using KontrolWork1.Domain;

namespace KontrolWork1.ImportExport;

public class CsvDataExporter : IDataExporterVisitor
{
    private readonly StringBuilder _sb = new StringBuilder();
    
    public string Export(IEnumerable<BankAccount> accounts, IEnumerable<Category> categories, IEnumerable<Operation> operations)
    {
        _sb.Clear();
        _sb.AppendLine("=== Accounts ===");
        foreach (var acc in accounts)
        {
            acc.Accept(this);
        }
        _sb.AppendLine("=== Categories ===");
        foreach (var cat in categories)
        {
            cat.Accept(this);
        }
        _sb.AppendLine("=== Operations ===");
        foreach (var op in operations)
        {
            op.Accept(this);
        }
        return _sb.ToString();
    }

    public void Visit(BankAccount account)
    {
        _sb.AppendLine($"{account.Id};{account.Name};{account.Balance}");
    }

    public void Visit(Category category)
    {
        _sb.AppendLine($"{category.Id};{category.Type};{category.Name}");
    }

    public void Visit(Operation operation)
    {
        _sb.AppendLine($"{operation.Id};{operation.Type};{operation.BankAccountId};{operation.Amount};{operation.Date};{operation.CategoryId};{operation.Description}");
    }
}