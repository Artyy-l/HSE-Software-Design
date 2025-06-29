using System.Text;
using KontrolWork1.Domain;

namespace KontrolWork1.ImportExport;

public class YamlDataExporter : IDataExporterVisitor
{
    private readonly StringBuilder _sb = new StringBuilder();

    public string Export(IEnumerable<BankAccount> accounts, IEnumerable<Category> categories, IEnumerable<Operation> operations)
    {
        _sb.Clear();
        _sb.AppendLine("accounts:");
        foreach (var acc in accounts)
        {
            acc.Accept(this);
        }
        _sb.AppendLine("categories:");
        foreach (var cat in categories)
        {
            cat.Accept(this);
        }
        _sb.AppendLine("operations:");
        foreach (var op in operations)
        {
            op.Accept(this);
        }
        return _sb.ToString();
    }

    public void Visit(BankAccount account)
    {
        _sb.AppendLine($"  - id: {account.Id}");
        _sb.AppendLine($"    name: {account.Name}");
        _sb.AppendLine($"    balance: {account.Balance}");
    }

    public void Visit(Category category)
    {
        _sb.AppendLine($"  - id: {category.Id}");
        _sb.AppendLine($"    type: {category.Type}");
        _sb.AppendLine($"    name: {category.Name}");
    }

    public void Visit(Operation operation)
    {
        _sb.AppendLine($"  - id: {operation.Id}");
        _sb.AppendLine($"    type: {operation.Type}");
        _sb.AppendLine($"    bankAccountId: {operation.BankAccountId}");
        _sb.AppendLine($"    amount: {operation.Amount}");
        _sb.AppendLine($"    date: {operation.Date}");
        _sb.AppendLine($"    categoryId: {operation.CategoryId}");
        _sb.AppendLine($"    description: {operation.Description}");
    }
}