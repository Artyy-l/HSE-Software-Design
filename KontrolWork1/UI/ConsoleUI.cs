using KontrolWork1.Commands;
using KontrolWork1.Domain;
using KontrolWork1.ImportExport;
using KontrolWork1.Managers;
using KontrolWork1.Menu;

namespace KontrolWork1.UI;

/// <summary>
/// Консольный пользовательский интерфейс для управления финансовым приложением.
/// </summary>
public partial class ConsoleUI
{
    private readonly BankAccountManager _accountManager;
    private readonly CategoryManager _categoryManager;
    private readonly OperationManager _operationManager;
    private readonly AnalyticsManager _analyticsManager;
    private readonly DataManagementManager _dataManagementManager;
    private readonly IDomainFactory _factory;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ConsoleUI"/>.
    /// </summary>
    public ConsoleUI(BankAccountManager accountManager,
                     CategoryManager categoryManager,
                     OperationManager operationManager,
                     AnalyticsManager analyticsManager,
                     DataManagementManager dataManagementManager,
                     IDomainFactory factory)
    {
        _accountManager = accountManager;
        _categoryManager = categoryManager;
        _operationManager = operationManager;
        _analyticsManager = analyticsManager;
        _dataManagementManager = dataManagementManager;
        _factory = factory;
    }

    /// <summary>
    /// Запускает основной цикл работы приложения.
    /// </summary>
    public void Run()
    {
        bool exit = false;
        while (!exit)
        {
            IAutoButton[] autoButtons = new IAutoButton[]
            {
                new AutoToggleButton(CreateAccount, "Создать счёт", "green"),
                new AutoToggleButton(CreateCategory, "Создать категорию", "green"),
                new AutoToggleButton(CreateOperation, "Создать операцию", "green"),
                new AutoToggleButton(ShowAnalytics, "Аналитика (доходы/расходы)", "green"),
                new AutoToggleButton(RecalculateBalance, "Пересчитать баланс", "green"),
                new AutoToggleButton(ExportData, "Экспорт данных", "green"),
                new AutoToggleButton(ImportData, "Импорт данных", "green"),
                new AutoToggleButton(DisplayAllData, "Вывести все данные", "green"),
                new AutoToggleButton(() => { exit = true; }, "Выход", "red")
            };

            AutoMenuButtons menu = new AutoMenuButtons(autoButtons, "Финансовое приложение");
            menu.ShowMenuAndExecute();
        }
    }

    /// <summary>
    /// Получает от пользователя непустую строку.
    /// </summary>
    private string GetNonEmptyString(string prompt)
    {
        string input;
        do
        {
            Console.WriteLine(prompt);
            input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                Console.WriteLine("Значение не может быть пустым. Попробуйте снова.");
        } while (string.IsNullOrWhiteSpace(input));
        return input;
    }

    /// <summary>
    /// Получает от пользователя неотрицательное число.
    /// </summary>
    private decimal GetPositiveDecimal(string prompt)
    {
        decimal value;
        while (true)
        {
            Console.WriteLine(prompt);
            string input = Console.ReadLine();
            if (decimal.TryParse(input, out value) && value >= 0)
                return value;
            Console.WriteLine("Введите неотрицательное число.");
        }
    }

    /// <summary>
    /// Получает от пользователя значение типа uint в указанном диапазоне.
    /// </summary>
    private uint GetUIntFromUser(string prompt, uint min, uint max)
    {
        uint value;
        while (true)
        {
            Console.WriteLine($"{prompt} (от {min} до {max}):");
            string input = Console.ReadLine();
            if (uint.TryParse(input, out value) && value >= min && value <= max)
                return value;
            Console.WriteLine($"Введите число от {min} до {max}.");
        }
    }

    /// <summary>
    /// Выводит меню с вариантами "Да/Нет" и возвращает выбор пользователя.
    /// </summary>
    private bool PromptYesNo(string prompt)
    {
        IButton[] buttons = new IButton[]
        {
            new ToggleButton("Да", "green"),
            new ToggleButton("Нет", "red")
        };
        MenuButtons menu = new MenuButtons(prompt, buttons);
        int selected = menu.ShowMenuAndGetOption(out _);
        return selected == 0;
    }

    /// <summary>
    /// Выводит меню с вариантами и возвращает индекс выбранного варианта.
    /// </summary>
    private int PromptForOption(string prompt, string[] options)
    {
        IButton[] buttons = options.Select(opt => new ToggleButton(opt, "green")).ToArray();
        MenuButtons menu = new MenuButtons(prompt, buttons);
        return menu.ShowMenuAndGetOption(out _);
    }

    /// <summary>
    /// Выводит список объектов с нумерацией.
    /// </summary>
    private void ShowListWithNumbers<T>(IEnumerable<T> items, string header)
    {
        Console.Clear();
        Console.WriteLine(header);
        int index = 1;
        foreach (var item in items)
            Console.WriteLine($"{index++}. {item}");
        Console.WriteLine("Нажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    /// <summary>
    /// Создаёт новый счёт с вводом названия и начального баланса.
    /// </summary>
    private void CreateAccount()
    {
        Console.Clear();
        string name = GetNonEmptyString("Введите название счёта:");
        decimal balance = GetPositiveDecimal("Введите начальный баланс (неотрицательное число):");
        try
        {
            ICommand cmd = new ExecutionTimeDecorator(new CreateAccountCommand(_accountManager, name, balance));
            cmd.Execute();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка создания счёта: " + ex.Message);
        }
    }

    /// <summary>
    /// Создаёт новую категорию с вводом названия и выбором типа.
    /// </summary>
    private void CreateCategory()
    {
        Console.Clear();
        string name = GetNonEmptyString("Введите название категории:");
        int typeOption = PromptForOption("Выберите тип категории:", new string[] { "Income", "Expense" });
        TransactionType type = typeOption == 0 ? TransactionType.Income : TransactionType.Expense;
        try
        {
            Category cat = _categoryManager.CreateCategory(type, name);
            Console.WriteLine("Создана категория: " + cat);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка создания категории: " + ex.Message);
        }
    }

    /// <summary>
    /// Создаёт новую операцию с выбором счёта и категории, а также проверкой суммы.
    /// </summary>
    private void CreateOperation()
    {
        Console.Clear();
        var accounts = _accountManager.GetAllAccounts().ToList();
        if (!accounts.Any())
        {
            Console.WriteLine("Нет доступных счетов. Сначала создайте счёт.");
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
            return;
        }
        var categories = _categoryManager.GetAllCategories().ToList();
        if (!categories.Any())
        {
            Console.WriteLine("Нет доступных категорий. Сначала создайте категорию.");
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
            return;
        }

        if (PromptYesNo("Хотите посмотреть список счетов?"))
            ShowListWithNumbers(accounts, "Список счетов:");

        uint accIndex = GetUIntFromUser("Введите номер счёта", 1, (uint)accounts.Count);
        BankAccount account = accounts[(int)accIndex - 1];

        if (PromptYesNo("Хотите посмотреть список категорий?"))
            ShowListWithNumbers(categories, "Список категорий:");

        uint catIndex = GetUIntFromUser("Введите номер категории", 1, (uint)categories.Count);
        Category category = categories[(int)catIndex - 1];

        decimal amount;
        while (true)
        {
            amount = GetPositiveDecimal("Введите сумму операции:");
            if (category.Type == TransactionType.Expense && amount > account.Balance)
                Console.WriteLine($"Ошибка: баланс счёта {account.Balance}. Сумма расхода не может превышать баланс. Попробуйте снова.");
            else break;
        }

        string description = GetNonEmptyString("Введите описание операции (или '-' для пропуска):");
        if (description.Trim() == "-") description = "";

        try
        {
            Operation op = _operationManager.CreateOperation(category.Type, account, amount, DateTime.Now, category, description);
            Console.WriteLine("Создана операция: " + op);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при создании операции: " + ex.Message);
        }
    }

    /// <summary>
    /// Выполняет анализ данных по заданному периоду.
    /// </summary>
    private void ShowAnalytics()
    {
        Console.Clear();
        DateTime start, end;
        while (true)
        {
            Console.WriteLine("Введите начальную дату (yyyy-MM-dd):");
            if (DateTime.TryParse(Console.ReadLine(), out start))
                break;
            Console.WriteLine("Некорректная дата. Попробуйте снова.");
        }
        while (true)
        {
            Console.WriteLine("Введите конечную дату (yyyy-MM-dd):");
            if (DateTime.TryParse(Console.ReadLine(), out end))
                break;
            Console.WriteLine("Некорректная дата. Попробуйте снова.");
        }
        decimal diff = _analyticsManager.CalculateBalanceDifference(start, end);
        Console.WriteLine($"Разница доходов и расходов с {start:yyyy-MM-dd} по {end:yyyy-MM-dd}: {diff}");
    }

    /// <summary>
    /// Пересчитывает балансы всех счетов.
    /// </summary>
    private void RecalculateBalance()
    {
        Console.Clear();
        ICommand recalcCommand = new ExecutionTimeDecorator(new RecalculateBalanceCommand(_dataManagementManager));
        recalcCommand.Execute();
        Console.WriteLine("Новые балансы:");
        foreach (var acc in _accountManager.GetAllAccounts())
            Console.WriteLine(acc);
    }

    /// <summary>
    /// Экспортирует данные в выбранном формате.
    /// </summary>
    private void ExportData()
    {
        Console.Clear();
        int option = PromptForOption("Выберите формат экспорта:", new string[] { "CSV", "JSON", "YAML" });
        switch (option)
        {
            case 0:
                {
                    var csvExporter = new CsvDataExporter();
                    string csv = csvExporter.Export(_accountManager.GetAllAccounts(), _categoryManager.GetAllCategories(), _operationManager.GetAllOperations());
                    Console.WriteLine("Экспорт в CSV:");
                    Console.WriteLine(csv);
                    break;
                }
            case 1:
                {
                    var jsonExporter = new JsonDataExporter();
                    jsonExporter.Export(_accountManager.GetAllAccounts(), _categoryManager.GetAllCategories(), _operationManager.GetAllOperations());
                    break;
                }
            case 2:
                {
                    var yamlExporter = new YamlDataExporter();
                    string yaml = yamlExporter.Export(_accountManager.GetAllAccounts(), _categoryManager.GetAllCategories(), _operationManager.GetAllOperations());
                    Console.WriteLine("Экспорт в YAML:");
                    Console.WriteLine(yaml);
                    break;
                }
            default:
                Console.WriteLine("Неверный выбор формата.");
                break;
        }
    }

    /// <summary>
    /// Импортирует данные из выбранного файла.
    /// </summary>
    private void ImportData()
    {
        Console.Clear();
        int option = PromptForOption("Выберите формат импорта:", new string[] { "CSV", "JSON", "YAML" });
        DataImporterBase importer = null;
        switch (option)
        {
            case 0:
                importer = new CsvDataImporter();
                break;
            case 1:
                importer = new JsonDataImporter();
                break;
            case 2:
                importer = new YamlDataImporter();
                break;
            default:
                Console.WriteLine("Неверный выбор формата.");
                return;
        }
        string path = GetNonEmptyString("Введите путь к файлу:");
        try
        {
            importer.Import(path, _accountManager, _categoryManager, _operationManager);
            Console.WriteLine("Импорт завершён успешно!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка импорта: " + ex.Message);
        }
    }

    /// <summary>
    /// Выводит все данные: счета, категории, операции.
    /// </summary>
    private void DisplayAllData()
    {
        Console.Clear();
        Console.WriteLine("Счета:");
        foreach (var acc in _accountManager.GetAllAccounts())
            Console.WriteLine(acc);
        Console.WriteLine("\nКатегории:");
        foreach (var cat in _categoryManager.GetAllCategories())
            Console.WriteLine(cat);
        Console.WriteLine("\nОперации:");
        foreach (var op in _operationManager.GetAllOperations())
            Console.WriteLine(op);
        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }
}
