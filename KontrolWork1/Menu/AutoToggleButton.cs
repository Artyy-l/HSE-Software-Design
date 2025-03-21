namespace KontrolWork1.Menu;

public class AutoToggleButton : ToggleButton, IAutoButton
{
    private Action _command;

    public Action Command
    {
        get => _command;
        set
        {
            _command = value ?? throw new ArgumentNullException("Command cannot be null");
        }
    }

    public AutoToggleButton()
    {
        _command = ()  => { };
    }

    public AutoToggleButton(Action command, string text = "Это кнопка", string highlightColor = "blue")
    {
        _command = command;
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
}