using KontrolWork1.Managers;

namespace KontrolWork1.Commands;

/// <summary>
/// Команда для создания нового банковского счёта.
/// </summary>
public class CreateAccountCommand : ICommand
{
    private readonly BankAccountManager _accountManager;
    private readonly string _name;
    private readonly decimal _balance;

    /// <summary>
    /// Инициализирует новую команду создания счёта.
    /// </summary>
    /// <param name="accountManager">Менеджер счётов.</param>
    /// <param name="name">Название нового счёта.</param>
    /// <param name="balance">Начальный баланс (неотрицательное число).</param>
    public CreateAccountCommand(BankAccountManager accountManager, string name, decimal balance)
    {
        _accountManager = accountManager;
        _name = name;
        _balance = balance;
    }

    /// <summary>
    /// Выполняет создание счёта и выводит результат в консоль.
    /// </summary>
    public void Execute()
    {
        var account = _accountManager.CreateAccount(_name, _balance);
        Console.WriteLine("Создан счёт: " + account);
    }
}