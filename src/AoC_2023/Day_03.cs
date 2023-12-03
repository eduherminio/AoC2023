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

    private readonly List<List<Number>> _numbers;
    private readonly List<List<Symbol>> _symbols;

    public Day_03()
    {
        (_numbers, _symbols) = ParseInput();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_OptimizedParsing()}");

    public override ValueTask<string> Solve_2() => new($"{Solve_2_OptimizedParsing()}");

    public int Solve_1_Original()
    {
        // Adaptation to mimic previous parsing style
        var numbers = _numbers.SelectMany(l => l);
        var symbols = _symbols.SelectMany(l => l).ToList();

        int result = 0;

        double diagonalDistance = Math.Sqrt(2) + double.Epsilon;

        foreach (var number in numbers)
        {
            if (symbols.Exists(symbol => number.Positions.Exists(numberPos => numberPos.DistanceTo(symbol.Position) <= diagonalDistance)))
            {
                result += number.Value;
            }
        }

        return result;
    }

    public int Solve_1_OptimizedParsing()
    {
        int result = 0;

        var lines = _numbers.Count;
        double diagonalDistance = Math.Sqrt(2) + double.Epsilon;

        for (int y = 0; y < lines; ++y)
        {
            foreach (var number in _numbers[y])
            {
                var min = y <= 0 ? 0 : y - 1;
                var max = y >= lines - 1 ? lines - 1 : y + 1;

                for (int symbolIndex = min; symbolIndex <= max; ++symbolIndex)
                {
                    foreach (var symbol in _symbols[symbolIndex])
                    {
                        foreach (var position in number.Positions)
                        {
                            if (position.DistanceTo(symbol.Position) <= diagonalDistance)
                            {
                                result += number.Value;
                                goto NextNumber;
                            }
                        }
                    }
                }

                NextNumber:;
            }
        }

        return result;
    }

    public ulong Solve_2_Original()
    {
        // Adaptation to mimic previous parsing style
        var numbers = _numbers.SelectMany(l => l).ToList();
        var symbols = _symbols.SelectMany(l => l).ToList();

        ulong result = 0;

        double diagonalDistance = Math.Sqrt(2) + double.Epsilon;

        foreach (var symbol in symbols)
        {
            var adjacentNumbers = numbers.Where(number => number.Positions.Exists(numberPos => numberPos.DistanceTo(symbol.Position) <= diagonalDistance)).ToList();

            if (adjacentNumbers.Count == 2)
            {
                result += (ulong)(adjacentNumbers[0].Value * adjacentNumbers[1].Value);
            }
        }

        return result;
    }

    public ulong Solve_2_OptimizedParsing()
    {
        ulong result = 0;

        double diagonalDistance = Math.Sqrt(2) + double.Epsilon;
        var lines = _numbers.Count;

        for (int y = 0; y < lines; ++y)
        {
            foreach (var symbol in _symbols[y])
            {
                var min = y <= 0 ? 0 : y - 1;
                var max = y >= lines - 1 ? lines - 1 : y + 1;

                List<int> partNumbers = new(32);

                for (int numberIndex = min; numberIndex <= max; ++numberIndex)
                {
                    foreach (var number in _numbers[numberIndex])
                    {
                        foreach (var position in number.Positions)
                        {
                            if (position.DistanceTo(symbol.Position) <= diagonalDistance)
                            {
                                partNumbers.Add(number.Value);
                                break;
                            }
                        }
                    }
                }

                if (partNumbers.Count == 2)
                {
                    result += (ulong)(partNumbers[0] * partNumbers[1]);
                }
            }
        }

        return result;
    }

    private (List<List<Number>> Numbers, List<List<Symbol>> Symbols) ParseInput()
    {
        var allLines = File.ReadAllLines(InputFilePath);

        List<List<Number>> numbers = new(allLines.Length);
        List<List<Symbol>> symbols = new(allLines.Length);

        Number? previousNumber = null;

        for (int y = 0; y < allLines.Length; ++y)
        {
            numbers.Add(new(allLines[y].Length));
            symbols.Add(new(allLines[y].Length));

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
                        symbols[y].Add(new(ch, new(x, y)));
                    }

                    if (previousNumber is not null)
                    {
                        numbers[y].Add(previousNumber);
                        previousNumber = null;
                    }
                }
            }

            if (previousNumber is not null)
            {
                numbers[y].Add(previousNumber);
                previousNumber = null;
            }
        }
        return (numbers, symbols);
    }
}
