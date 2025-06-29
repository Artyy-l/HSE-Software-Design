using KontrolWork1.Domain;
using KontrolWork1.Managers;

namespace KontrolWork1.ImportExport;

 /// <summary>
/// Импортер данных из CSV файлов.
/// </summary>
public class CsvDataImporter : DataImporterBase
{
    /// <summary>
    /// Парсит содержимое CSV файла в промежуточную модель.
    /// </summary>
    /// <param name="fileContent">Содержимое файла.</param>
    /// <returns>Коллекция строковых массивов.</returns>
    protected override dynamic ParseData(string fileContent)
    {
        var lines = fileContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var data = new List<string[]>();
        foreach (var line in lines)
        {
            var fields = line.Split(';');
            data.Add(fields);
        }
        return data;
    }

    /// <summary>
    /// Обрабатывает данные и создаёт объекты в системе.
    /// </summary>
    /// <param name="parsedData">Промежуточная модель данных.</param>
    /// <param name="accountManager">Менеджер счетов.</param>
    /// <param name="categoryManager">Менеджер категорий.</param>
    /// <param name="operationManager">Менеджер операций.</param>
    protected override void ProcessData(dynamic parsedData,
        BankAccountManager accountManager,
        CategoryManager categoryManager,
        OperationManager operationManager)
    {
        foreach (string[] fields in parsedData)
        {
            // Формат CSV: Type;Name;Value;[Date];[Description]
            // Type = "account", "category", "operation"
            string type = fields[0].Trim().ToLower();
            if (type == "account")
            {
                string name = fields[1].Trim();
                if (decimal.TryParse(fields[2], out decimal balance))
                {
                    // Создаём счёт с заданным балансом
                    accountManager.CreateAccount(name, balance);
                }
            }
            else if (type == "category")
            {
                string typeStr = fields[1].Trim().ToLower();
                TransactionType transType = typeStr == "income" ? TransactionType.Income : TransactionType.Expense;
                string name = fields[2].Trim();
                categoryManager.CreateCategory(transType, name);
            }
            else if (type == "operation")
            {
                // Ожидаем поля: operation;AccountName;CategoryName;Amount;Date;Description
                string accountName = fields[1].Trim();
                string categoryName = fields[2].Trim();
                if (decimal.TryParse(fields[3], out decimal amount) && DateTime.TryParse(fields[4], out DateTime date))
                {
                    string description = fields.Length >= 6 ? fields[5].Trim() : "";
                    var account = accountManager.GetAllAccounts().FirstOrDefault(a =>
                        a.Name.Equals(accountName, StringComparison.OrdinalIgnoreCase));
                    var category = categoryManager.GetAllCategories().FirstOrDefault(c =>
                        c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
                    if (account != null && category != null)
                    {
                        operationManager.CreateOperation(category.Type, account, amount, date, category, description);
                    }
                }
            }
        }
    }
}