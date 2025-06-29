namespace KontrolWork1.Domain;

/// <summary>
/// Класс, представляющий операцию (доход или расход).
/// </summary>
public class Operation
{
    /// <summary>
    /// Уникальный идентификатор операции.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Тип операции (доход или расход).
    /// </summary>
    public TransactionType Type { get; }

    /// <summary>
    /// Идентификатор счёта, к которому относится операция.
    /// </summary>
    public Guid BankAccountId { get; }

    /// <summary>
    /// Сумма операции.
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    /// Дата операции.
    /// </summary>
    public DateTime Date { get; }

    /// <summary>
    /// Описание операции.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Идентификатор категории, к которой относится операция.
    /// </summary>
    public Guid CategoryId { get; }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="Operation"/>.
    /// </summary>
    /// <param name="type">Тип операции.</param>
    /// <param name="bankAccountId">Идентификатор счёта.</param>
    /// <param name="amount">Сумма операции (должна быть положительной).</param>
    /// <param name="date">Дата операции.</param>
    /// <param name="categoryId">Идентификатор категории.</param>
    /// <param name="description">Описание операции.</param>
    public Operation(TransactionType type, Guid bankAccountId, decimal amount, DateTime date, Guid categoryId, string description = "")
    {
        if (amount <= 0)
            throw new ArgumentException("Сумма операции должна быть положительной.", nameof(amount));

        Id = Guid.NewGuid();
        Type = type;
        BankAccountId = bankAccountId;
        Amount = amount;
        Date = date;
        CategoryId = categoryId;
        Description = string.IsNullOrWhiteSpace(description) ? "" : description;
    }

    /// <summary>
    /// Преобразует первые 4 байта GUID в uint для удобства чтения.
    /// </summary>
    /// <returns>Человекочитаемое представление идентификатора.</returns>
    private uint GetUintId() => BitConverter.ToUInt32(Id.ToByteArray(), 0);

    /// <summary>
    /// Возвращает строковое представление операции.
    /// </summary>
    /// <returns>Строка в формате "[uint] Type: Amount on Date (Category: uint) Description".</returns>
    public override string ToString()
    {
        return $"[{GetUintId()}] {Type}: {Amount} on {Date.ToShortDateString()} (Category: {BitConverter.ToUInt32(CategoryId.ToByteArray(), 0)}) {Description}";
    }
}