namespace KontrolWork1.Menu;

/// <summary>
/// "Переключительная" кнопка - можно выбрать только одну из списка
/// </summary>
public class ToggleButton : IButton
{
    private protected readonly string _iconOff = "🔘";
    private protected readonly string[] _iconOn = { "🔵" };
    private protected string _text = "Это кнопка";
    private protected readonly string[] _colors = { "green", "yellow", "blue", "red", "purple" };
    private protected string _highlightColor = "blue";
    private protected string _selectedIcon = "🔘";
    private protected bool _isSelected = false;

    /// <summary>
    /// Иконка, которая отображена, если кнопка не нажата
    /// </summary>
    public string IconOff => _iconOff;

    /// <summary>
    /// Иконки, которые могут быть отображены, если кнопка нажата
    /// </summary>
    public string[] IconOn => _iconOn;

    /// <summary>
    /// Текст кнопки
    /// </summary>
    public string Text
    {
        get => _text;
        set
        {
            if (value == null || value.Length == 0 || value.Length > 100)
            {
                throw new ArgumentException("Текста либо нет, либо он длиннее 100 символов");
            }
            else
            {
                _text = value;
            }
        }
    }

    /// <summary>
    /// Доступные цвета текста для отображения при наведении на кнопку
    /// </summary>
    public string[] Colors => _colors;

    /// <summary>
    /// Цвет текста кнопки при наведении на неё
    /// </summary>
    public string HighlightColor
    {
        get => _highlightColor;
        set
        {
            if (value == null || value.Length == 0 || !_colors.Contains(value))
            {
                throw new ArgumentException("Недопустимый цвет");
            }
            else
            {
                _highlightColor = value;
            }
        }
    }

    /// <summary>
    /// Текущая иконка
    /// </summary>
    public string SelectedIcon => _selectedIcon;

    /// <summary>
    /// Нажата ли кнопка
    /// </summary>
    public bool IsSelected => _isSelected;

    /// <summary>
    /// Создаёт кнопку с надписью <paramref name="text"/> и <paramref name="highlightColor"/> подстветкой 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="highlightColor"></param>
    public ToggleButton(string text = "Это кнопка", string highlightColor = "blue")
    {
        Text = text;
        HighlightColor = highlightColor;
    }

    /// <summary>
    /// Возвращает иконку и текст кнопки
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{SelectedIcon}  {Text}";
    }

    /// <summary>
    /// Обработка нажатия на кнопку
    /// </summary>
    /// <param name="key"></param>
    /// <param name="isYouClick"></param>
    /// <param name="order"></param>
    /// <param name="typeOfClick"></param>
    /// <returns>
    /// <p>
    /// -1 => кнопку не включали и не выключали
    /// </p><p>
    /// 0 => кнопку выключили
    /// </p><p>
    /// 1 => кнопку включили
    /// </p>
    /// </returns>
    public virtual int ClickButton(ConsoleKeyInfo key, bool isYouClick, int typeOfClick)
    {
        if (key.Key == ConsoleKey.Spacebar)
        {
            if (isYouClick)
            {
                _isSelected = !_isSelected;
                _selectedIcon = (_isSelected ? _iconOn[0] : _iconOff);
                typeOfClick = (_isSelected ? 1 : 0);
                return typeOfClick;
            }
            else if (_isSelected && (typeOfClick == 1))
            {
                _isSelected = false;
                _selectedIcon = _iconOff;
                return 0;
            }
            else return -1;
        }
        return -1;
    }
}