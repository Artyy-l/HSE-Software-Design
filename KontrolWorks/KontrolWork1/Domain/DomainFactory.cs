namespace KontrolWork1.Domain;

/// <summary>
/// Фабрика доменных объектов.
/// </summary>
public class DomainFactory : IDomainFactory
{
    /// <inheritdoc/>
    public BankAccount CreateBankAccount(string name, decimal balance)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название счета не может быть пустым.", nameof(name));
        if (balance < 0)
            throw new ArgumentException("Баланс не может быть отрицательным.", nameof(balance));
        return new BankAccount(name, balance);
    }

    /// <inheritdoc/>
    public Category CreateCategory(TransactionType type, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название категории не может быть пустым.", nameof(name));
        return new Category(type, name);
    }

    /// <inheritdoc/>
    public Operation CreateOperation(TransactionType type, BankAccount account, decimal amount, DateTime date, Category category, string description = "")
    {
        if (account == null)
            throw new ArgumentNullException(nameof(account));
        if (category == null)
            throw new ArgumentNullException(nameof(category));
        if (type != category.Type)
            throw new InvalidOperationException("Тип операции не соответствует типу категории.");
        return new Operation(type, account.Id, amount, date, category.Id, description);
    }
}

