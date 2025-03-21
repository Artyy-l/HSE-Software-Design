using System.Diagnostics;

namespace KontrolWork1.Commands;

public class ExecutionTimeDecorator : ICommand
{
    private readonly ICommand _innerCommand;

    public ExecutionTimeDecorator(ICommand innerCommand)
    {
        _innerCommand = innerCommand;
    }

    public void Execute()
    {
        var stopwatch = Stopwatch.StartNew();
        _innerCommand.Execute();
        stopwatch.Stop();
        Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
    }
}