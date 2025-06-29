namespace KontrolWork1.Domain;

/// <summary>
/// Класс, представляющий категорию операции.
/// </summary>
public class Category
{
    /// <summary>
    /// Уникальный идентификатор категории.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Тип категории (доход или расход).
    /// </summary>
    public TransactionType Type { get; }

    /// <summary>
    /// Название категории.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="Category"/>.
    /// </summary>
    /// <param name="type">Тип категории.</param>
    /// <param name="name">Название категории. Не должно быть пустым.</param>
    public Category(TransactionType type, string name)
    {
        Id = Guid.NewGuid();
        Type = type;
        Name = name;
    }

    /// <summary>
    /// Преобразует первые 4 байта GUID в uint для удобства чтения.
    /// </summary>
    /// <returns>Человекочитаемое представление идентификатора.</returns>
    private uint GetUintId() => BitConverter.ToUInt32(Id.ToByteArray(), 0);

    /// <summary>
    /// Возвращает строковое представление категории.
    /// </summary>
    /// <returns>Строка в формате "[uint] Name (Type)".</returns>
    public override string ToString()
    {
        return $"[{GetUintId()}] {Name} ({Type})";
    }
}
