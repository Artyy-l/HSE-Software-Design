namespace KontrolWork1.Domain;

/// <summary>
/// Класс, представляющий банковский счёт.
/// </summary>
public class BankAccount
{
    /// <summary>
    /// Уникальный идентификатор счёта.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Название счёта.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Текущий баланс счёта.
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="BankAccount"/>.
    /// </summary>
    /// <param name="name">Название счёта. Не должно быть пустым.</param>
    /// <param name="balance">Начальный баланс (неотрицательное число).</param>
    public BankAccount(string name, decimal balance)
    {
        Id = Guid.NewGuid();
        Name = name;
        Balance = balance;
    }

    /// <summary>
    /// Преобразует первые 4 байта GUID в uint для удобства чтения.
    /// </summary>
    /// <returns>Человекочитаемое представление идентификатора.</returns>
    private uint GetUintId() => BitConverter.ToUInt32(Id.ToByteArray(), 0);

    /// <summary>
    /// Возвращает строковое представление счёта.
    /// </summary>
    /// <returns>Строка в формате "[uint] Name (Balance: value)".</returns>
    public override string ToString()
    {
        return $"[{GetUintId()}] {Name} (Balance: {Balance})";
    }
}