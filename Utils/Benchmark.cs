using System.Diagnostics;

namespace Lightning.Utils;

public static class Benchmark
{
    public static void Run(string message, Action action)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        action();
        stopwatch.Stop();
        Console.WriteLine(
            $"{message} {stopwatch.ElapsedMilliseconds}ms {stopwatch.ElapsedTicks}ts"
        );
    }

    public static T Run<T>(string message, Func<T> action)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var result = action();
        stopwatch.Stop();
        Console.WriteLine(
            $"{message} {stopwatch.ElapsedMilliseconds}ms {stopwatch.ElapsedTicks}ts"
        );
        return result;
    }
}
