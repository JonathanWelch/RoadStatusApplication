using RoadStatus.Domain.Interfaces;

namespace RoadStatus.Infrastructure;

public class ConsoleOutput : IOutput
{
    public virtual void WriteLine(string line)
    {
        Console.WriteLine(line);
    }
}