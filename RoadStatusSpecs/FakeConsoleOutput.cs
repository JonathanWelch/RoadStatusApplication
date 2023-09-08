using RoadStatus.Infrastructure;

namespace RoadStatusSpecs;

internal class FakeConsoleOutput : ConsoleOutput
{
    public readonly List<string> Lines;

    public FakeConsoleOutput()
    {
        Lines = new List<string>();
    }

    public override void WriteLine(string line)
    {
        Lines.Add(line);
    }
}