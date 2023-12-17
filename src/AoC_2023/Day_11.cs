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
    private readonly List<Point> _galaxies;

    public Day_11()
    {
        _galaxies = new(1024);
        _input = ParseInput().ToList();
        (_xGaps, _yGaps) = ExtractUniverseGaps();
    }

    public override ValueTask<string> Solve_1() => new($"{CalculateDistance(2)}");

    public override ValueTask<string> Solve_2() => new($"{CalculateDistance(1_000_000)}");

    private long CalculateDistance(int expansionFactor)
    {
        long result = 0;
        foreach (var subset in _galaxies.Subsets(2))
        {
            result += (long)subset[0].ManhattanDistance(subset[1]);

            var minX = Math.Min(subset[0].X, subset[1].X);
            var maxX = Math.Max(subset[0].X, subset[1].X);
            foreach (var x in _xGaps)
            {
                if (x > minX && x < maxX)
                {
                    result += expansionFactor - 1;
                }
            }

            var minY = Math.Min(subset[0].Y, subset[1].Y);
            var maxY = Math.Max(subset[0].Y, subset[1].Y);
            foreach (var y in _yGaps)
            {
                if (y > minY && y < maxY)
                {
                    result += expansionFactor - 1;
                }
            }
        }

        return result;
    }

    private (List<int> xGaps, List<int> yGaps) ExtractUniverseGaps()
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
                Point point = new(ch, x++, y);
                returnLine.Add(point);

                if (point.IsGalaxy)
                {
                    _galaxies.Add(point);
                }
            }

            y++;
            yield return returnLine;
        }
    }
}
