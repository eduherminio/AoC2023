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
        ExpandUniverse();

        // Print(_input);

        var galaxies = ExtractGalaxies();

        int result = 0;
        foreach (var subset in galaxies.Subsets(2))
        {
            result += (int)subset[0].ManhattanDistance(subset[1]);
        }

        return new($"{result}");
    }

    private void ExpandUniverse()
    {
        List<int> yGaps = new(_input.Count);
        List<int> xGaps = new(_input[0].Count);

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

        for (int y = 0; y < yGaps.Count; ++y)
        {
            var inputIndex = yGaps[y] + y;
            _input.Insert(inputIndex, _input[inputIndex].ToList());
        }

        var emptyPoint = new Point('.', -1, -1);

        for (int x = 0; x < xGaps.Count; ++x)
        {
            for (int y = 0; y < _input.Count; ++y)
            {
                _input[y].Insert(xGaps[x] + x, emptyPoint);
            }
        }
    }

    private List<Point> ExtractGalaxies()
    {
        var galaxies = new List<Point>(_input.Count / 10);

        for (int y = 0; y < _input.Count; ++y)
        {
            for (int x = 0; x < _input[0].Count; ++x)
            {
                if (_input[y][x].IsGalaxy)
                {
                    galaxies.Add(new('#', x, y));
                }
            }
        }

        return galaxies;
    }

    public override ValueTask<string> Solve_2()
    {
        int result = 0;

        return new($"{result}");
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
