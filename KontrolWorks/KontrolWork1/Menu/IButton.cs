namespace KontrolWork1.Menu;

public interface IButton
{
    public string IconOff { get; }
    public string[] IconOn { get; }
    public string SelectedIcon { get; }
    public bool IsSelected { get;}
    public string Text { get; set; }
    public string[] Colors { get; }
    public string HighlightColor { get; set; }
    public string ToString();
    public int ClickButton(ConsoleKeyInfo key, bool isYouClick, int typeOfClick);
}