using RoadStatus.Infrastructure;

namespace RoadStatusTests.Infrastructure;

[TestFixture]
public class ConsoleOutputTests
{
    [Test]
    public void CanWriteLineToConsole()
    {
        const string line = "A line of text";
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);
        var output = new ConsoleOutput();
            
        output.WriteLine(line);

        Assert.That(stringWriter.ToString(), Is.EqualTo($"{line}\r\n"));
    }
}