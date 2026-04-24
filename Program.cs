namespace ConsoleApp;


// Głowny program i wywołanie klas
internal class Program
{
    static void Main(string[] args)
    {
        // Bazowe paramtery, uzywane do pomiarów wydajnośći
        const int defaultSize = 100; // romziar macierzy
        const int defaultRepetitions = 10;  // liczba powtórzeń pętli
        
        var defaultThreadCounts = new[]
        {
            1,
            2,
            4,
            8, 
            Environment.ProcessorCount,
            Environment.ProcessorCount * 2,
            Environment.ProcessorCount * 4
        };

        // Arguemnty i Konsola: rozmiar, liczba prób, lista wątków i opcja podglądu macierzy.
        var size = ReadIntArgument(args, 0, defaultSize);
        var repetitions = ReadIntArgument(args, 1, defaultRepetitions);
        var threadCounts = ReadThreadCountsArgument(args, 2, defaultThreadCounts)
            .Prepend(1)
            .Distinct()
            .Order()
            .ToArray();

        var showMatrices = args.Any(arg => arg.Equals("--show", StringComparison.OrdinalIgnoreCase));

        Console.WriteLine("(Parallel vs Wątek)");
        Console.WriteLine($"Wymiar macierzy =>  {size} x {size}");
        Console.WriteLine($"Ile razy powtórzono? =>  {repetitions}");
        Console.WriteLine($"Uyte wątki =>  {string.Join(", ", threadCounts)}");
        Console.WriteLine();
        
        var left = Matrix.CreateRandom(size, size);
        var right = Matrix.CreateRandom(size, size);

        if (showMatrices && size <= 8)
        {
            left.Print("Macierz A:");
            right.Print("Macierz B:");
        }

        // Pobudzenie, bo JIT moze przy starcie spowodować gorsze wyniki.
        Alghoritms.MultiplyParallel(left, right, 1);
        Alghoritms.MultiplyThreaded(left, right, 1);
        Console.WriteLine();

        // pomiar dla Wątków i Paraller.
        var parallelRows = Mensuring.Run(left, right, threadCounts, repetitions, Alghoritms.MultiplyParallel);
        var threadRows = Mensuring.Run(left, right, threadCounts, repetitions, Alghoritms.MultiplyThreaded);
        var reference = parallelRows.First(row => row.Threads == 1).Result;

        PrintStrategyTable("Parallel", parallelRows, reference);
        PrintStrategyTable("Thread", threadRows, reference);
        PrintComparisonTable(parallelRows, threadRows);

        if (showMatrices && size <= 8)
        {
            reference.Print("Macierz wynikowa:");
        }
    }

    // Argumenty 
    static int ReadIntArgument(string[] args, int index, int defaultValue)
    {
        if (args.Length <= index)
        {
            return defaultValue;
        }

        return int.TryParse(args[index], out var value) && value > 0 ? value : defaultValue;
    }

    // Liczba wątków np. "1,2,4,8".
    static int[] ReadThreadCountsArgument(string[] args, int index, int[] defaultValue)
    {
        if (args.Length <= index)
        {
            return defaultValue;
        }

        var parsed = args[index]
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(part => int.TryParse(part, out var value) && value > 0 ? value : 0)
            .Where(value => value > 0)
            .ToArray();

        return parsed.Length == 0 ? defaultValue : parsed;
    }

    // Wypisuje tabelę wyników dla jednej strategii obliczeń.
    static void PrintStrategyTable(string strategyName, List<BenchmarkRow> rows, Matrix reference)
    {
        Console.WriteLine($"Wyniki - {strategyName}:");
        Console.WriteLine($"{"Wątki",8} | {"[ms]",18} | {"Który szybszy?",15} | Czy zgodne z wątkiem?");
        Console.WriteLine(new string('-', 75));

        var baseline = rows.First(row => row.Threads == 1).AverageMilliseconds;

        foreach (var row in rows)
        {
            var isCorrect = row.Result.IsEqualTo(reference) ? "tak" : "nie";
            var speedup = baseline / row.AverageMilliseconds;
            Console.WriteLine($"{row.Threads,8} | {row.AverageMilliseconds,18:F2} | {speedup,15:F2} | {isCorrect}");
        }

        Console.WriteLine();
    }

    // orównanie średnich czasów Parallel i Thread.
    static void PrintComparisonTable(List<BenchmarkRow> parallelRows, List<BenchmarkRow> threadRows)
    {
        Console.WriteLine("Porównanie Parallel vs Thread:");
        Console.WriteLine($"{"Wątki",8} | {"Parallel [ms]",14} | {"Thread [ms]",12} | Szybsze podejście");
        Console.WriteLine(new string('-', 66));

        var threadByCount = threadRows.ToDictionary(row => row.Threads);

        foreach (var parallelRow in parallelRows)
        {
            if (!threadByCount.TryGetValue(parallelRow.Threads, out var threadRow))
            {
                continue;
            }

            var winner = parallelRow.AverageMilliseconds <= threadRow.AverageMilliseconds ? "Parallel" : "Thread";
            Console.WriteLine($"{parallelRow.Threads,8} | {parallelRow.AverageMilliseconds,14:F2} | {threadRow.AverageMilliseconds,12:F2} | {winner}");
        }

        Console.WriteLine();
    }
}