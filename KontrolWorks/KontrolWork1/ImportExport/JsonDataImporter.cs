using System.Text.Json;
using KontrolWork1.Domain;
using KontrolWork1.Managers;

namespace KontrolWork1.ImportExport;

public class JsonDataImporter : DataImporterBase
{
    protected override dynamic ParseData(string fileContent)
    {
        return JsonSerializer.Deserialize<dynamic>(fileContent);
    }

    protected override void ProcessData(dynamic parsedData, 
                                        BankAccountManager accountManager, 
                                        CategoryManager categoryManager, 
                                        OperationManager operationManager)
    {
        foreach (var acc in parsedData.accounts)
        {
            string name = acc.name;
            decimal balance = acc.balance;
            accountManager.CreateAccount(name, balance);
        }

        foreach (var cat in parsedData.categories)
        {
            string typeStr = cat.type.ToString().ToLower();
            TransactionType transType = typeStr == "income" ? TransactionType.Income : TransactionType.Expense;
            string name = cat.name;
            categoryManager.CreateCategory(transType, name);
        }

        foreach (var op in parsedData.operations)
        {
            string accountName = op.accountName;
            string categoryName = op.categoryName;
            decimal amount = op.amount;
            DateTime date = op.date;
            string description = op.description;
            var account = accountManager.GetAllAccounts().FirstOrDefault(a => a.Name.Equals(accountName, StringComparison.OrdinalIgnoreCase));
            var category = categoryManager.GetAllCategories().FirstOrDefault(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
            if (account != null && category != null)
            {
                operationManager.CreateOperation(category.Type, account, amount, date, category, description);
            }
        }
    }
}