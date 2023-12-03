using SheepTools.Model;

namespace AoC_2023;

public class Day_03 : BaseDay
{
    private sealed record Number
    {
        public List<IntPoint> Positions { get; }
        public int Value { get; set; }

        public Number()
        {
            Positions = new List<IntPoint>(8);
        }
    }

    private sealed record Symbol(char Id, IntPoint Position);

    private readonly List<Number> _numbers;
    private readonly List<Symbol> _symbols;

    public Day_03()
    {
        (_numbers, _symbols) = ParseInput();
    }

    public override ValueTask<string> Solve_1()
    {
        int result = 0;

        double diagonalDistance = Math.Sqrt(2) + double.Epsilon;

        foreach (var number in _numbers)
        {
            if (_symbols.Exists(symbol => number.Positions.Exists(numberPos => numberPos.DistanceTo(symbol.Position) <= diagonalDistance)))
            {
                result += number.Value;
            }
        }

        return new($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        ulong result = 0;

        double diagonalDistance = Math.Sqrt(2) + double.Epsilon;

        foreach (var symbol in _symbols)
        {
            var adjacentNumbers = _numbers.Where(number => number.Positions.Exists(numberPos => numberPos.DistanceTo(symbol.Position) <= diagonalDistance)).ToList();

            if (adjacentNumbers.Count == 2)
            {
                result += (ulong)(adjacentNumbers[0].Value * adjacentNumbers[1].Value);
            }
        }

        return new($"{result}");
    }

    private (List<Number> Numbers, List<Symbol> Symbols) ParseInput()
    {
        List<Number> numbers = new(1024);
        List<Symbol> symbols = new(1024);
        Number? previousNumber = null;

        var allLines = File.ReadAllLines(InputFilePath);

        for (int y = 0; y < allLines.Length; ++y)
        {
            for (int x = allLines[y].Length - 1; x >= 0; --x)
            {
                var ch = allLines[y][x];

                if (char.IsDigit(ch))
                {
                    previousNumber ??= new();
                    previousNumber.Value += (ch - '0') * (int)Math.Pow(10, previousNumber.Positions.Count);
                    previousNumber.Positions.Add(new(x, y));
                }
                else
                {
                    if (ch != '.')
                    {
                        symbols.Add(new(ch, new(x, y)));
                    }

                    if (previousNumber is not null)
                    {
                        numbers.Add(previousNumber);
                        previousNumber = null;
                    }
                }
            }

            if (previousNumber is not null)
            {
                numbers.Add(previousNumber);
                previousNumber = null;
            }
        }
        return (numbers, symbols);
    }
}
