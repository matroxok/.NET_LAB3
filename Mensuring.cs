namespace ConsoleApp;
using System.Diagnostics;


// Część odpowiedzialna za pomiary (wyjadność)
public sealed record BenchmarkRow(int Threads, double AverageMilliseconds, Matrix Result);

public class Mensuring
{
    public static List<BenchmarkRow> Run(
        Matrix left,
        Matrix right,
        int[] threadCounts,
        int repetitions,
        Func<Matrix, Matrix, int, Matrix> multiply)
    {
        if (repetitions <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(repetitions));
        }

        var results = new List<BenchmarkRow>();

        foreach (var threads in threadCounts)
        {
            var stopwatchTotal = 0.0;
            Matrix? lastResult = null;

            for (var run = 0; run < repetitions; run++)
            {
                var stopwatch = Stopwatch.StartNew();
                lastResult = multiply(left, right, threads);
                stopwatch.Stop();

                stopwatchTotal += stopwatch.Elapsed.TotalMilliseconds;
            }

            results.Add(new BenchmarkRow(threads, stopwatchTotal / repetitions, lastResult!));
        }

        return results;
    }
}