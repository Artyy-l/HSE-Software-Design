using System.Text;

namespace KontrolWork1.Menu;

/// <summary>
/// Объект Меню с кнопками
/// </summary>
public class MenuButtons
{
    private protected string _title = "Это меню";
    private readonly IButton[] _buttons;
    private protected int _length = 0;
    private protected int _highlightedButton = 0;
    private protected List<int> _selectedButtons = new List<int> { -1, -1, -1, -1, -1, -1, -1, -1, -1};


    /// <summary>
    /// Заголовок Меню - от 1 до 30 символов
    /// </summary>
    public string Title
    {
        get => _title;
        set
        {
            if (value == null || value.Length == 0 || value.Length > 100)
            {
                throw new ArgumentException("Названия либо нет, либо оно длиннее 100 символов");
            }
            else
            {
                _title = value;
            }
        }
    }

    /// <summary>
    /// Кнопки, которые есть в меню
    /// </summary>
    public IButton[] Buttons => _buttons;

    /// <summary>
    /// Количество кнопок в меню
    /// </summary>
    public int Length => _length;

    /// <summary>
    /// Меню из одной кнопки ToggleButton с параметрами по умолчанию
    /// </summary>
    public MenuButtons()
    {
        _buttons = new IButton[] { new ToggleButton() };
    }

    /// <summary>
    /// Создаём Меню с заголовком <paramref name="title"/> и кнопками <paramref name="buttons"/>.
    /// Кнопки должны быть одного типа!
    /// </summary>
    /// <param name="title"></param>
    /// <param name="buttons"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public MenuButtons(string title, IButton[] buttons)
    {
        Title = title;
        if (buttons == null || buttons.Length == 0)
        {
            throw new ArgumentNullException("В меню обязательно должны быть кнопки");
        }
        else if (!buttons.All(button => button.GetType() == buttons[0].GetType()))
        {
            throw new ArgumentException("Кнопки в меню должны быть одного типа");
        }
        _buttons = buttons;
        _length = buttons.Length;
    }

    /// <summary>
    /// Выводим Меню в консоль
    /// </summary>
    public virtual void PrintMenu()
    {
        Console.OutputEncoding = Encoding.Unicode;

        // Очищаем экран
        Console.Clear();
        Console.WriteLine("\x1b[3J");

        int maxLength = Title.Length;
        for (int i = 0; i < Length; i++)
        {
            // Пример кнопки: "🔘  Это кнопка"
            maxLength = Math.Max(maxLength, Buttons[i].ToString().Length + 3);
        }

        int amountOfStrings = 4 + Length;

        // установка курсора
        int centerX = (Console.WindowWidth / 2) - (maxLength / 2) - 6;
        int centerY = (Console.WindowHeight - amountOfStrings) / 2;
        if (centerX < 0 || centerY < 0)
        {
            Console.WriteLine("\x1b[46mУвеличьте размер окна, пожалуйста, затем тыкните на что-нибудь\x1b[0m");
            Console.ReadKey();
            return;
        }
        Console.SetCursorPosition(centerX, centerY++);

        // выводим заголовок
        Console.Write($"╔{new string('═', maxLength + 10)}╗");
        Console.SetCursorPosition(centerX, centerY++);
        Console.Write($"║{Title.PadLeft((maxLength + Title.Length + 10) / 2).PadRight(maxLength + 10)}║");
        Console.SetCursorPosition(centerX, centerY++);
        Console.Write($"╠{new string('═', maxLength + 10)}╣");
        Console.SetCursorPosition(centerX, centerY++);

        // выводим кнопки
        for (int i = 0; i < Length; i++)
        {
            if (i == _highlightedButton)
            {
                Console.Write($"║ {Buttons[i].SelectedIcon}  ");
                Console.ForegroundColor = ConvertToConsoleColor(Buttons[i].HighlightColor);
                Console.Write(Buttons[i].Text);
                Console.ResetColor();
                Console.Write($"{(new string(' ', maxLength + 10 - Buttons[i].ToString().Length - 1))}║"); 
            }
            else
            {
                Console.Write($"║ {Buttons[i]}{new string(' ', maxLength + 10 - Buttons[i].ToString().Length - 1)}║");
            }
            Console.SetCursorPosition(centerX, centerY++);
        }
        Console.Write($"╚{new string('═', maxLength + 10)}╝");
        Console.SetCursorPosition(0, 0);
    }

    /// <summary>
    /// Конвертируем цвет подсвеченной кнопки <paramref name="colorString"/> из строки в ConsoleColor
    /// </summary>
    /// <param name="colorString"></param>
    /// <returns></returns>
    public static ConsoleColor ConvertToConsoleColor(string colorString)
    {
        switch (colorString.ToLower())
        {
            case "black":
                return ConsoleColor.Black;
            case "blue":
                return ConsoleColor.Blue;
            case "cyan":
                return ConsoleColor.Cyan;
            case "darkblue":
                return ConsoleColor.DarkBlue;
            case "darkcyan":
                return ConsoleColor.DarkCyan;
            case "darkgray":
                return ConsoleColor.DarkGray;
            case "darkgreen":
                return ConsoleColor.DarkGreen;
            case "darkmagenta":
                return ConsoleColor.DarkMagenta;
            case "darkred":
                return ConsoleColor.DarkRed;
            case "darkyellow":
                return ConsoleColor.DarkYellow;
            case "gray":
                return ConsoleColor.Gray;
            case "green":
                return ConsoleColor.Green;
            case "magenta":
                return ConsoleColor.Magenta;
            case "red":
                return ConsoleColor.Red;
            case "white":
                return ConsoleColor.White;
            case "yellow":
                return ConsoleColor.Yellow;
            default:
                return ConsoleColor.White; 
        }
    }

    /// <summary>
    /// Двигаем курсор, чтобы пользователь мог выбрать опцию
    /// </summary>
    /// <returns>Индекс опции, которую выбрал пользователь, или -1, если он её не выбрал</returns>
    public virtual int MoveCursor()
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        switch (keyInfo.Key)
        {
            case ConsoleKey.UpArrow:
                _highlightedButton = (Length + _highlightedButton - 1) % Length;
                return -1;

            case ConsoleKey.DownArrow:
                _highlightedButton = (_highlightedButton + 1) % Length;
                return -1;

            case ConsoleKey.Enter:

                foreach (var button in _selectedButtons)
                {
                    if (button != -1) return _highlightedButton;
                }
                Console.WriteLine("\x1b[46mВы не выбрали ни одну кнопку. Для продолжения нажмите на что-нибудь\x1b[0m");
                Console.ReadKey();
                return -1;

            default:
                int typeOfClick = Buttons[_highlightedButton].ClickButton(keyInfo, true, -1);

                for (int i = 0; i < 9; i++)
                {
                    if (_selectedButtons[i] == _highlightedButton || _selectedButtons[i] == -1)
                        continue;
                    int change = Buttons[_selectedButtons[i]].ClickButton(keyInfo, false, typeOfClick);
                    AddRemoveButtonFromList(i, change);
                }
                AddRemoveButtonFromList(_highlightedButton, typeOfClick);
                return -1;
        }
    }

    private protected virtual void AddRemoveButtonFromList(int number, int operation)
    {
        if (operation == -2)
        {
            for (int i = 0; i < 9; i++)
            {
                if (_selectedButtons[i] == -1) { _selectedButtons[i] = number; }
            }
        }
        if (operation == 0)
        {
            for (int i = 0; i < 9; i++)
            {
                if (_selectedButtons[i] == number) { _selectedButtons[i] = -1; }
            }
        }

        else if (operation >= 1)
        {
            _selectedButtons[operation - 1] = number;
        }
    }

    /// <summary>
    /// Выводим Меню на экран, позволяем пользователю выбрать опцию и возвращаем её индекс
    /// </summary>
    /// <returns></returns>
    public virtual int ShowMenuAndGetOption(out int option)
    {
        option = -1;
        while (option == -1)
        {
            PrintMenu();
            option = MoveCursor();
        }
        Console.Clear();
        return option;
    }
}