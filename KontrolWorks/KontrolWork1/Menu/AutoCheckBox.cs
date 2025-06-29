namespace KontrolWork1.Menu;

public class AutoCheckBox : CheckBox, IAutoButton
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

    public AutoCheckBox()
    {
        _command = () => { };
    }

    public AutoCheckBox(Action command, string text = "Это кнопка", string highlightColor = "blue")
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