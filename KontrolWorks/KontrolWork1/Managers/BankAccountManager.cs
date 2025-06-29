using KontrolWork1.Data;
using KontrolWork1.Domain;

namespace KontrolWork1.Managers;

/// <summary>
/// Класс-менеджер для управления банковскими счетами.
/// </summary>
public class BankAccountManager
{
    private readonly IRepository<BankAccount> _accountRepository;
    private readonly IDomainFactory _factory;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="BankAccountManager"/>.
    /// </summary>
    /// <param name="accountRepository">Репозиторий для хранения счетов.</param>
    /// <param name="factory">Фабрика для создания доменных объектов.</param>
    public BankAccountManager(IRepository<BankAccount> accountRepository, IDomainFactory factory)
    {
        _accountRepository = accountRepository;
        _factory = factory;
    }

    /// <summary>
    /// Создаёт новый банковский счёт с заданным названием и начальным балансом.
    /// Баланс должен быть неотрицательным.
    /// </summary>
    /// <param name="name">Название счёта.</param>
    /// <param name="balance">Начальный баланс (неотрицательное число).</param>
    /// <returns>Созданный объект <see cref="BankAccount"/>.</returns>
    public BankAccount CreateAccount(string name, decimal balance)
    {
        var account = _factory.CreateBankAccount(name, balance);
        _accountRepository.Add(account);
        return account;
    }

    /// <summary>
    /// Обновляет данные указанного банковского счёта.
    /// </summary>
    /// <param name="account">Обновлённый объект счёта.</param>
    public void UpdateAccount(BankAccount account)
    {
        _accountRepository.Add(account);
    }

    /// <summary>
    /// Удаляет счёт по его идентификатору.
    /// </summary>
    /// <param name="accountId">Идентификатор счёта.</param>
    public void DeleteAccount(Guid accountId)
    {
        _accountRepository.Remove(accountId);
    }

    /// <summary>
    /// Возвращает все существующие банковские счета.
    /// </summary>
    /// <returns>Перечисление объектов <see cref="BankAccount"/>.</returns>
    public IEnumerable<BankAccount> GetAllAccounts()
    {
        return _accountRepository.GetAll();
    }
}