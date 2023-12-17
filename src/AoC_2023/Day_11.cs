using MoreLinq;
using SheepTools.Model;

namespace AoC_2023;
public class Day_11 : BaseDay
{
    private sealed record Point : IntPointWithValue<char>
    {
        public Point(char value, int x, int y) : base(x, y)
        {
            Value = value;
        }

        public bool IsGalaxy => Value == '#';
    }

    private readonly List<List<Point>> _input;

    private readonly List<int> _xGaps;
    private readonly List<int> _yGaps;

    public Day_11()
    {
        _input = ParseInput().ToList();
        (_xGaps, _yGaps) = ExtractUniverseGaps();
    }

    public override ValueTask<string> Solve_1()
    {
        var expandedInput = ExpandUniverse([.. _input.ConvertAll(l => l.ToList())]);

        // Print(_input);

        var galaxies = ExtractGalaxies(expandedInput);

        int result = 0;
        foreach (var subset in galaxies.Subsets(2))
        {
            result += (int)subset[0].ManhattanDistance(subset[1]);
        }

        return new($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        const int expansionFactor = 1_000_000;

        // Print(expandedInput);

        var galaxies = ExtractGalaxies(_input);

        long result = 0;
        foreach (var subset in galaxies.Subsets(2))
        {
            result +=(long)subset[0].ManhattanDistance(subset[1]);

            var minX = Math.Min(subset[0].X, subset[1].X);
            var maxX = Math.Max(subset[0].X, subset[1].X);
            for (int x = minX + 1; x < maxX; ++x)
            {
                if (_xGaps.Contains(x))
                {
                    result += expansionFactor - 1;
                }
            }

            var minY = Math.Min(subset[0].Y, subset[1].Y);
            var maxY = Math.Max(subset[0].Y, subset[1].Y);
            for (int y = minY + 1; y < maxY; ++y)
            {
                if (_yGaps.Contains(y))
                {
                    result += expansionFactor - 1;
                }
            }
        }

        return new($"{result}");
    }

    private List<List<Point>> ExpandUniverse(List<List<Point>> universe)
    {
        for (int y = 0; y < _yGaps.Count; ++y)
        {
            var inputIndex = _yGaps[y] + y;
            universe.Insert(inputIndex, [.. universe[inputIndex]]);
        }

        var emptyPoint = new Point('.', -1, -1);

        for (int x = 0; x < _xGaps.Count; ++x)
        {
            for (int y = 0; y < universe.Count; ++y)
            {
                universe[y].Insert(_xGaps[x] + x, emptyPoint);
            }
        }

        return universe;
    }

    private List<List<Point>> ExpandUniverse_Part2(List<List<Point>> universe)
    {
        const int expansionFactor = 99;

        // Expand on y
        for (int y = 0; y < _yGaps.Count; ++y)
        {
            for (int i = 0; i < expansionFactor; ++i)
            {
                var inputIndex = _yGaps[y] + y * expansionFactor;
                universe.Insert(inputIndex, [.. universe[inputIndex]]);
            }
        }

        var emptyPoint = new Point('.', -1, -1);

        Print(universe);

        // Expand on x
        for (int x = 0; x < _xGaps.Count; ++x)
        {
            for (int y = 0; y < universe.Count; ++y)
            {
                for (int i = 0; i < expansionFactor; ++i)
                {
                    int inputIndex = _xGaps[x] + x * expansionFactor;
                    universe[y].Insert(inputIndex, emptyPoint);
                }
            }
        }

        return universe;
    }

    private  (List<int> xGaps, List<int> yGaps) ExtractUniverseGaps()
    {
        List<int> yGaps = new(_input.Count);
        for (int y = 0; y < _input.Count; ++y)
        {
            bool isGalaxy = false;
            for (int x = 0; x < _input[0].Count; ++x)
            {
                if (_input[y][x].IsGalaxy)
                {
                    isGalaxy = true;
                    break;
                }
            }

            if (!isGalaxy)
            {
                yGaps.Add(y);
            }
        }

        List<int> xGaps = new(_input[0].Count);
        for (int x = 0; x < _input[0].Count; ++x)
        {
            bool isGalaxy = false;
            for (int y = 0; y < _input.Count; ++y)
            {
                if (_input[y][x].IsGalaxy)
                {
                    isGalaxy = true;
                    break;
                }
            }

            if (!isGalaxy)
            {
                xGaps.Add(x);
            }
        }

        return (xGaps, yGaps);
    }

    private static List<Point> ExtractGalaxies(List<List<Point>> universe)
    {
        var galaxies = new List<Point>(universe.Count / 10);

        for (int y = 0; y < universe.Count; ++y)
        {
            for (int x = 0; x < universe[0].Count; ++x)
            {
                if (universe[y][x].IsGalaxy)
                {
                    galaxies.Add(new('#', x, y));
                }
            }
        }

        return galaxies;
    }

    private IEnumerable<List<Point>> ParseInput()
    {
        var file = new ParsedFile(InputFilePath);
        int y = 0;

        while (!file.Empty)
        {
            var line = file.NextLine();
            var returnLine = new List<Point>(2 * line.Count);

            int x = 0;
            foreach (var ch in line.ToList<char>())
            {
                returnLine.Add(new(ch, x++, y));
            }

            y++;
            yield return returnLine;
        }
    }

    private static void Print<T>(IEnumerable<IEnumerable<IntPointWithValue<T>>> grid)
    {
        for (int y = 0; y < grid.Count(); ++y)
        {
            for (int x = 0; x < grid.ElementAt(y).Count(); ++x)
            {
                Console.Write(grid.ElementAt(y).ElementAt(x).Value);
            }
            Console.WriteLine();
        }
    }
}
