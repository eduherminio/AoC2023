namespace AoC_2023;

internal sealed record Map(ulong DestinationRangeStart, ulong SourceRangeStart, ulong RangeLength)
{
    public ulong To(ulong seed)
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
    private sealed record Input(List<ulong> InitialSeeds, List<List<Map>> FromToMaps);

    private readonly Input _input;

    public Day_05()
    {
        _input = ParseInput();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_Original()}");

    public override ValueTask<string> Solve_2() => new($"");

    public ulong Solve_1_Original()
    {
        return _input.InitialSeeds.Min(seed =>
            _input.FromToMaps.Aggregate(seed, (current, next) => next.GroupBy(m => m.To(current)).OrderByDescending(g => g.Count()).Last().Key));
    }

    public ulong Solve_2_Original_NoArray()
    {
        List<ulong> seeds = new List<ulong>(_input.InitialSeeds.Count * 1000);

        for (int i = 0; i < _input.InitialSeeds.Count - 1; i += 2)
        {
            for (int j = 0; j < (int)_input.InitialSeeds[i + 1]; ++j)
            {
                seeds.Add(_input.InitialSeeds[i] + (ulong)j);
            }
        }

        return seeds.Min(seed =>
            _input.FromToMaps.Aggregate(seed, (current, next) => next.GroupBy(m => m.To(current)).OrderByDescending(g => g.Count()).Last().Key));
    }

    private static readonly object _lockObj = new object();

    public ulong Solve_2_NoArray_Foreach()
    {
        IEnumerable<ulong> InitialSeeds()
        {
            for (int i = 0; i < _input.InitialSeeds.Count - 1; i += 2)
            {
                for (int j = 0; j < (int)_input.InitialSeeds[i + 1]; ++j)
                {
                    yield return _input.InitialSeeds[i] + (ulong)j;
                }
            }
        }

        ulong index = 0;
        var minLocation = ulong.MaxValue;
        foreach (var seed in InitialSeeds())
        {
            if(index % 100_000 == 0)
            {
                Console.WriteLine(index);
            }

            var location = _input.FromToMaps.Aggregate(seed, (current, next) => next.GroupBy(m => m.To(current)).OrderByDescending(g => g.Count()).Last().Key);
            if (location < minLocation)
            {
                minLocation = location;
            }
            ++index;
        }

        return minLocation;
    }

    public ulong Solve_2_NoArray_ParallelForeach()
    {
        IEnumerable<ulong> InitialSeeds()
        {
            for (int i = 0; i < _input.InitialSeeds.Count - 1; i += 2)
            {
                for (int j = 0; j < (int)_input.InitialSeeds[i + 1]; ++j)
                {
                    yield return _input.InitialSeeds[i] + (ulong)j;
                }
            }
        }

        var minLocation = ulong.MaxValue;
        ulong index = 0;

        Parallel.ForEach(InitialSeeds(), seed =>
        {
            if (index % 100_000 == 0)
            {
                Console.WriteLine(index);
            }

            var location = _input.FromToMaps.Aggregate(seed, (current, next) => next.GroupBy(m => m.To(current)).OrderByDescending(g => g.Count()).Last().Key);
            lock (_lockObj)
            {
                if (location < minLocation)
                {
                    minLocation = location;
                }
            }

            Interlocked.Increment(ref index);
        });

        return minLocation;
    }

    private Input ParseInput()
    {
        var groupOfLines = ParsedFile.ReadAllGroupsOfLines(InputFilePath);
        var seedStrings = groupOfLines[0][0].Split(' ', StringSplitOptions.RemoveEmptyEntries);

        List<ulong> seeds = new(seedStrings.Length - 1);
        for (int i = 1; i < seedStrings.Length; ++i)
        {
            seeds.Add(ulong.Parse(seedStrings[i]));
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

                input.FromToMaps.Last().Add(new(ulong.Parse(mapStrings[0]), ulong.Parse(mapStrings[1]), ulong.Parse(mapStrings[2])));
            }
        }

        return input;
    }
}
