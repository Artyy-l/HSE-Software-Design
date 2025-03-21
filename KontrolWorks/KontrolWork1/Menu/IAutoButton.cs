namespace KontrolWork1.Menu;

public interface IAutoButton : IButton
{
    public Action Command { get; set; }
}