namespace KontrolWork1.Domain;

/// <summary>
/// Интерфейс фабрики доменных объектов.
/// </summary>
public interface IDomainFactory
{
    /// <summary>
    /// Создаёт новый банковский счёт с указанным названием и начальным балансом (неотрицательным).
    /// </summary>
    /// <param name="name">Название счёта.</param>
    /// <param name="balance">Начальный баланс (неотрицательное число).</param>
    /// <returns>Новый объект BankAccount.</returns>
    BankAccount CreateBankAccount(string name, decimal balance);

    /// <summary>
    /// Создаёт новую категорию с указанным типом и названием.
    /// </summary>
    /// <param name="type">Тип категории.</param>
    /// <param name="name">Название категории.</param>
    /// <returns>Новый объект Category.</returns>
    Category CreateCategory(TransactionType type, string name);

    /// <summary>
    /// Создаёт новую операцию с заданными параметрами.
    /// </summary>
    /// <param name="type">Тип операции.</param>
    /// <param name="account">Счёт, к которому относится операция.</param>
    /// <param name="amount">Сумма операции (должна быть положительной).</param>
    /// <param name="date">Дата операции.</param>
    /// <param name="category">Категория операции.</param>
    /// <param name="description">Описание операции.</param>
    /// <returns>Новый объект Operation.</returns>
    Operation CreateOperation(TransactionType type, BankAccount account, decimal amount, DateTime date, Category category, string description = "");
}