namespace AoC_2023;

internal sealed record Map(long DestinationRangeStart, long SourceRangeStart, long RangeLength)
{
    public long To(long seed)
    {
        if (seed >= SourceRangeStart)
        {
            var diff = seed - SourceRangeStart;

            if (diff < RangeLength)
            {
                return DestinationRangeStart + diff;
            }
        }

        return seed;
    }
}

public class Day_05 : BaseDay
{
    private sealed record Input(List<long> InitialSeeds, List<List<Map>> FromToMaps);

    private readonly Input _input;

    public Day_05()
    {
        _input = ParseInput();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_Original()}");

    public override ValueTask<string> Solve_2() => new($"{Solve_2_Original()}");

    public long Solve_1_Original()
    {
        // .OrderByDescending(g => g.Count()).Last(): Trick to being able to use LINQ,
        // providing there are at least 3 maps per layer and they don't overlap:
        // only one affects the seed and produces a different result

        return _input.InitialSeeds.Min(seed =>
            _input.FromToMaps.Aggregate(seed, (current, next) =>
                next.GroupBy(m => m.To(current))
                .OrderByDescending(g => g.Count())
                .Last()
                .Key));
    }

    public long Solve_2_Original()
    {
        var min = long.MaxValue;
        var max = 0L;

        foreach (var mapLayer in _input.FromToMaps)
        {
            foreach (var map in mapLayer)
            {
                if (map.SourceRangeStart < min)
                {
                    min = map.SourceRangeStart;
                }

                long end = map.SourceRangeStart + map.RangeLength;
                if (end > max)
                {
                    max = end;
                }
            }
        }

        int maxArrayLength = (int)Math.Min(max - min, Array.MaxLength);
        var arraysNeeded = 1 + ((int)((max - min) / maxArrayLength));

        var currentArrays = new bool[arraysNeeded][];
        for (int i = 0; i < arraysNeeded; i++)
        {
            currentArrays[i] = (new bool[maxArrayLength]);
        }

        foreach (var initialSeed in InitialSeeds())
        {
            var arrayIndex = (int)Math.DivRem(initialSeed, maxArrayLength, out var itemIndex);
            currentArrays[arrayIndex][itemIndex] = true;
        }

        foreach (var mapLayer in _input.FromToMaps)
        {
            var nextArrays = new bool[arraysNeeded][];
            for (int i = 0; i < arraysNeeded; i++)
            {
                nextArrays[i] = (new bool[maxArrayLength]);
            }

            foreach (var map in mapLayer)
            {
                for (var i = map.SourceRangeStart; i < map.SourceRangeStart + map.RangeLength; ++i)
                {
                    var currentArrayIndex = (int)Math.DivRem(i, maxArrayLength, out var currentItemIndex);

                    if (currentArrays[currentArrayIndex][currentItemIndex])
                    {
                        currentArrays[currentArrayIndex][currentItemIndex] = false;   // To be able to distinguish the unaffectd ones

                        var valueToTransform = ((long)currentArrayIndex * maxArrayLength) + currentItemIndex;
                        var newItemValue = map.To(valueToTransform);
                        var nextArrayIndex = (int)Math.DivRem(newItemValue, maxArrayLength, out var nextItemIndex);

                        nextArrays[nextArrayIndex][nextItemIndex] = true;
                    }
                }
            }

            // Letting through those ones unaffected by the mapping layer
            for (int arrayIndex = 0; arrayIndex < currentArrays.Length; ++arrayIndex)
            {
                for (int itemIndex = 0; itemIndex < currentArrays[arrayIndex].Length; ++itemIndex)
                {
                    if (currentArrays[arrayIndex][itemIndex])
                    {
                        nextArrays[arrayIndex][itemIndex] = true;
                    }
                }
            }

            currentArrays = nextArrays;
        }

        for (int i = 0; i < currentArrays.Length; ++i)
        {
            for (int j = 0; j < currentArrays[i].Length; ++j)
            {
                if (currentArrays[i][j])
                {
                    return (i * (long)maxArrayLength) + j;
                }
            }
        }

        throw new SolvingException();

        IEnumerable<long> InitialSeeds()
        {
            for (int i = 0; i < _input.InitialSeeds.Count - 1; i += 2)
            {
                for (int j = 0; j < (int)_input.InitialSeeds[i + 1]; ++j)
                {
                    yield return _input.InitialSeeds[i] + j;
                }
            }
        }
    }

    private Input ParseInput()
    {
        var groupOfLines = ParsedFile.ReadAllGroupsOfLines(InputFilePath);
        var seedStrings = groupOfLines[0][0].Split(' ', StringSplitOptions.RemoveEmptyEntries);

        List<long> seeds = new(seedStrings.Length - 1);
        for (int i = 1; i < seedStrings.Length; ++i)
        {
            seeds.Add(long.Parse(seedStrings[i]));
        }

        var input = new Input(seeds, new(groupOfLines.Count - 1));

        for (int i = 1; i < groupOfLines.Count; ++i)
        {
            input.FromToMaps.Add(new(groupOfLines[i].Count));

            for (int j = 1; j < groupOfLines[i].Count; ++j)
            {
                var line = groupOfLines[i][j];

                var mapStrings = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (mapStrings.Length != 3)
                {
                    throw new SolvingException();
                }

                input.FromToMaps.Last().Add(new(long.Parse(mapStrings[0]), long.Parse(mapStrings[1]), long.Parse(mapStrings[2])));
            }
        }

        return input;
    }
}
