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

    public Day_11()
    {
        _input = ParseInput().ToList();
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
        var expandedInput = ExpandUniverse_Part2([.. _input.ConvertAll(l => l.ToList())]);

        // Print(expandedInput);

        var galaxies = ExtractGalaxies(expandedInput);

        int result = 0;
        foreach (var subset in galaxies.Subsets(2))
        {
            result += (int)subset[0].ManhattanDistance(subset[1]);
        }

        return new($"{result}");
    }

    private static List<List<Point>> ExpandUniverse(List<List<Point>> universe)
    {
        List<int> yGaps = new(universe.Count);
        List<int> xGaps = new(universe[0].Count);

        for (int y = 0; y < universe.Count; ++y)
        {
            bool isGalaxy = false;
            for (int x = 0; x < universe[0].Count; ++x)
            {
                if (universe[y][x].IsGalaxy)
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

        for (int x = 0; x < universe[0].Count; ++x)
        {
            bool isGalaxy = false;
            for (int y = 0; y < universe.Count; ++y)
            {
                if (universe[y][x].IsGalaxy)
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

        for (int y = 0; y < yGaps.Count; ++y)
        {
            var inputIndex = yGaps[y] + y;
            universe.Insert(inputIndex, [.. universe[inputIndex]]);
        }

        var emptyPoint = new Point('.', -1, -1);

        for (int x = 0; x < xGaps.Count; ++x)
        {
            for (int y = 0; y < universe.Count; ++y)
            {
                universe[y].Insert(xGaps[x] + x, emptyPoint);
            }
        }

        return universe;
    }

    private static List<List<Point>> ExpandUniverse_Part2(List<List<Point>> universe)
    {
        const int expandFactor = 999_999;

        List<int> yGaps = new(universe.Count);
        for (int y = 0; y < universe.Count; ++y)
        {
            bool isGalaxy = false;
            for (int x = 0; x < universe[0].Count; ++x)
            {
                if (universe[y][x].IsGalaxy)
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

        List<int> xGaps = new(universe[0].Count);
        for (int x = 0; x < universe[0].Count; ++x)
        {
            bool isGalaxy = false;
            for (int y = 0; y < universe.Count; ++y)
            {
                if (universe[y][x].IsGalaxy)
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

        // Expand on y
        for (int y = 0; y < yGaps.Count; ++y)
        {
            for (int i = 0; i < expandFactor; ++i)
            {
                var inputIndex = yGaps[y] + y * expandFactor;
                universe.Insert(inputIndex, [.. universe[inputIndex]]);
            }
        }

        var emptyPoint = new Point('.', -1, -1);

        Print(universe);

        // Expand on x
        for (int x = 0; x < xGaps.Count; ++x)
        {
            for (int y = 0; y < universe.Count; ++y)
            {
                for (int i = 0; i < expandFactor; ++i)
                {
                    int inputIndex = xGaps[x] + x * expandFactor;
                    universe[y].Insert(inputIndex, emptyPoint);
                }
            }
        }

        return universe;
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
