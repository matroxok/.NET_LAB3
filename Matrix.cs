namespace ConsoleApp;


// Część odpowiedzialna za Macierze.
public class Matrix
{
    private readonly int[,] values;
    public Matrix(int rows, int columns)
    {
        if (rows <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rows));
        }

        if (columns <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(columns));
        }

        Rows = rows;
        Columns = columns;
        values = new int[rows, columns];
    }

    public int Rows { get; }

    public int Columns { get; }

    public int this[int row, int column]
    {
        get => values[row, column];
        set => values[row, column] = value;
    }

    // zakres minValue, maxValue.
    public static Matrix CreateRandom(int rows, int columns, int minValue = 0, int maxValue = 10)
    {
        var matrix = new Matrix(rows, columns);

        for (var row = 0; row < rows; row++)
        {
            for (var column = 0; column < columns; column++)
            {
                matrix[row, column] = Random.Shared.Next(minValue, maxValue);
            }
        }

        return matrix;
    }

    // Logowanie macierzy w konsoli 
    public void Print(string title)
    {
        Console.WriteLine(title);

        for (var row = 0; row < Rows; row++)
        {
            for (var column = 0; column < Columns; column++)
            {
                Console.Write($"{this[row, column],4}");
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    // Porównanie macierzy względem siebie
    public bool IsEqualTo(Matrix other)
    {
        if (Rows != other.Rows || Columns != other.Columns)
        {
            return false;
        }

        for (var row = 0; row < Rows; row++)
        {
            for (var column = 0; column < Columns; column++)
            {
                if (this[row, column] != other[row, column])
                {
                    return false;
                }
            }
        }

        return true;
    }
}