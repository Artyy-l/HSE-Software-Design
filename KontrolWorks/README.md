# Отчет по КР-1

## Инструкция по запуску приложения
Особых инструкций нет, нужно собрать и запустить проект. При этом для выбора опции в меню, нужно нажать кнопку с помощью ПРОБЕЛА и зафиксировать выбор при помощи ENTER

## Общая идея решения
Консольное приложение для учета финансов, которое позволяет:
- Создавать, редактировать и удалять банковские счета, категории и операции (доходы/расходы).
- Выполнять аналитику: расчет разницы доходов и расходов, группировка по категориям.
- Импортировать и экспортировать данные в формате CSV, YAML, JSON.
- Пересчитывать баланс при несоответствиях.

Функциональные требования не изменялись относительно исходного задания.

## Принципы SOLID и GRASP

- **SRP (Принцип единственной ответственности):**  
  Классы `BankAccount`, `Category` и `Operation` отвечают только за хранение данных и базовую бизнес-логику.

- **OCP (Принцип открытости/закрытости):**  
  Добавление новых форматов импорта/экспорта возможно без изменения существующего кода, благодаря абстрактному классу `DataImporterBase`.

- **DIP (Принцип инверсии зависимостей):**  
  Используются абстракции `IRepository<T>` и `IDomainFactory`, которые передаются через конструкторы.

- **High Cohesion и Low Coupling (GRASP):**  
  Фасады `BankAccountManager`, `CategoryManager` и `OperationManager` группируют логику, минимизируя зависимость между модулями.

## Реализованные паттерны GoF

### Фабрика
Используется для централизованного создания объектов и их валидации.  
Реализовано в `DomainFactory`:

```csharp
public BankAccount CreateBankAccount(string name, decimal balance)
{
    // доп проверки
    return new BankAccount(name, balance);
}

## Фасад
Объединяет логику работы с банковскими счетами, упрощая использование.
Реализовано в `BankAccountManager` (есть нестыковочка по названию, но это фасад вроде) и других:

```csharp
public BankAccount CreateAccount(string name, decimal balance)
{
    var account = _factory.CreateBankAccount(name, balance);
    _accountRepository.Add(account);
    return account;
}
```

## Команда и Декоратор
Каждый пользовательский сценарий представлен командой, а декоратор добавляет функциональность измерения времени.
Реализовано в `ExecutionTimeDecorator`:

```csharp
public class ExecutionTimeDecorator : ICommand
{
    private readonly ICommand _innerCommand;

    public ExecutionTimeDecorator(ICommand innerCommand)
    {
        _innerCommand = innerCommand;
    }

    public void Execute()
    {
        var stopwatch = Stopwatch.StartNew();
        _innerCommand.Execute();
        stopwatch.Stop();
        Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
    }
}
```

## Шаблонный метод
Определяет общий алгоритм импорта данных, позволяя переопределять только парсинг.
Реализовано в `DataImporterBase`:

```csharp
public abstract class DataImporterBase
{
    public void Import(string filePath, BankAccountManager accountManager, CategoryManager categoryManager, OperationManager operationManager)
    {
        var fileContent = ReadFile(filePath);
        var parsedData = ParseData(fileContent);
        ProcessData(parsedData, accountManager, categoryManager, operationManager);
    }
    
    protected virtual string ReadFile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Файл не найден.", filePath);
        return File.ReadAllText(filePath);
    }
    
    protected abstract dynamic ParseData(string fileContent);
    protected abstract void ProcessData(dynamic parsedData, BankAccountManager accountManager, CategoryManager categoryManager, 
        OperationManager operationManager);
}
```

## Посетитель
Позволяет добавлять новые операции с объектами без изменения их кода.
Реализовано в расширении `ExportableExtensions` методом `Accept`:

```csharp
public static void Accept(this BankAccount account, IDataExporterVisitor visitor)
{
    visitor.Visit(account);
}
```

И меню красивое для консольного приложения на приколе добавлено (лучше бы код доработала конечно, ну ладна...)
