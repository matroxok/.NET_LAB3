namespace ConsoleApp;


// część odpwiedzialna za algorytmy pomiarowe
// - Parallel
// Thread
public class Alghoritms
{   
    public static Matrix MultiplyThreaded(Matrix left, Matrix right, int threadCount)
    {
        ValidateInput(left, right, threadCount);

        var result = new Matrix(left.Rows, right.Columns);
        var workers = Math.Min(threadCount, left.Rows);
        var threads = new Thread[workers];

        var baseRowsPerThread = left.Rows / workers;
        var extraRows = left.Rows % workers;
        var startRow = 0;

        for (var workerIndex = 0; workerIndex < workers; workerIndex++)
        {
            var rowsForCurrentThread = baseRowsPerThread + (workerIndex < extraRows ? 1 : 0);
            var localStart = startRow;
            var localEnd = localStart + rowsForCurrentThread;
            startRow = localEnd;

            threads[workerIndex] = new Thread(() =>
            {
                MultiplyRowRange(left, right, result, localStart, localEnd);
            });
        }

        foreach (var thread in threads)
        {
            thread.Start();
        }

        // synchronizuja konieca pracy: główny wątek czeka na wszystkie wątki pozostale
        foreach (var thread in threads)
        {
            thread.Join();
        }

        return result;
    }

    // Parallel
    public static Matrix MultiplyParallel(Matrix left, Matrix right, int maxDegreeOfParallelism)
    {
        ValidateInput(left, right, maxDegreeOfParallelism);

        var result = new Matrix(left.Rows, right.Columns);
        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = maxDegreeOfParallelism
        };

        Parallel.For(0, left.Rows, options, row =>
        {
            MultiplyRowRange(left, right, result, row, row + 1);
        });

        return result;
    }

    // Sprawdzamy wymiary oraz poprawność liczby wątków.
    private static void ValidateInput(Matrix left, Matrix right, int threadCount)
    {
        if (left.Columns != right.Rows)
        {
            throw new ArgumentException("lewo musi byc prawo (macierz)");
        }

        if (threadCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(threadCount));
        }
    }

    // Liczy wynik mnożenia dla podanego zakresu.
    private static void MultiplyRowRange(Matrix left, Matrix right, Matrix result, int startRowInclusive, int endRowExclusive)
    {
        for (var row = startRowInclusive; row < endRowExclusive; row++)
        {
            for (var column = 0; column < right.Columns; column++)
            {
                var sum = 0;

                for (var index = 0; index < left.Columns; index++)
                {
                    sum += left[row, index] * right[index, column];
                }

                result[row, column] = sum;
            }
        }
    }
}