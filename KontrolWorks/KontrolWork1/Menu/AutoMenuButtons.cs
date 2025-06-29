using System.Text;

namespace KontrolWork1.Menu;

public class AutoMenuButtons : MenuButtons
{
    private IAutoButton[] _buttons;

    /// <summary>
    /// Кнопки, которые есть в меню
    /// </summary>
    public new IAutoButton[] Buttons => _buttons;

    public AutoMenuButtons(IAutoButton[] buttons, string title = "Это меню")
    {
        Title = title;
        _buttons = buttons;
        _length = buttons.Length;
    }

    public void Execute()
    {

        for (int i = 0; i < 9; i++)
        {
            if (_selectedButtons[i] != -1)
            {
                Buttons[_selectedButtons[i]].Command();
            }
        }
    }

    /// <summary>
    /// Выводим Меню в консоль
    /// </summary>
    public new void PrintMenu()
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
    /// Двигаем курсор, чтобы пользователь мог выбрать опцию
    /// </summary>
    /// <returns>Индекс опции, которую выбрал пользователь, или -1, если он её не выбрал</returns>
    public new int MoveCursor()
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
                    if (button != -1) return 0;
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

    private protected new void AddRemoveButtonFromList(int number, int operation)
    {
        if (operation == -2)
        {
            for (int i = 0; i < 9; i++)
            {
                if (_selectedButtons[i] == -1) { _selectedButtons[i] = number; break; }
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
    public new int ShowMenuAndGetOption(out int option)
    {
        option = -1;
        while (option != 0)
        {
            PrintMenu();
            option = MoveCursor();
        }
        Console.Clear();
        return option;
    }

    public void ShowMenuAndExecute()
    {
        ShowMenuAndGetOption(out int option);
        Execute();
    }
}