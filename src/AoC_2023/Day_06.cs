namespace AoC_2023;

public class Day_06 : BaseDay
{
    private readonly List<(int Time, int Distance)> _input;

    public Day_06()
    {
        _input = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        long result = 1;

        foreach (var (inputTime, inputDistance) in _input)
        {
            result *= CountRecordsWithBinarySearch(inputTime, inputDistance);
        }

        return new($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        string inputTimeString = "";
        string inputDistanceString = "";
        foreach (var (time, distance) in _input)
        {
            inputTimeString += time.ToString();
            inputDistanceString += distance.ToString();
        }

        int inputTime = int.Parse(inputTimeString);
        long inputDistance = long.Parse(inputDistanceString);

        var result = CountRecordsWithBinarySearch(inputTime, inputDistance);

        return new($"{result}");
    }

    internal static long CountRecords(int inputTime, long inputDistance)
    {
        long occurrences = 0;

        for (int t = 0; t < inputTime; ++t)
        {
            long speed = t;
            var time = inputTime - t;

            long distance = speed * time;

            if (distance > inputDistance)
            {
                ++occurrences;
            }
        }

        return occurrences;
    }

    internal static long CountRecordsWithEarlyBreak(int inputTime, long inputDistance, int tToStart = 1)
    {
        long occurrences = 0;

        for (int t = tToStart; t < inputTime; ++t)
        {
            long speed = t;
            var time = inputTime - t;

            long distance = speed * time;

            if (distance > inputDistance)
            {
                ++occurrences;
            }
            else if (occurrences > 0)
            {
                break;
            }
        }

        return occurrences;
    }

    internal static long CountRecordsWithBinarySearch(int inputTime, long inputDistance)
    {
        // y = x(inputTime - x)
        // y = x(inputTime) - x^2

        // Los puntos máximo y mínimo están en los extremos o en las raíces obtenidas de igualar la derivada a 0
        // dy = inputTime - 2x
        // dy = 0 -> inputTime = 2x -> x = inputTime/2

        // Los extremos son 0, así que no pueden ser el máximo, ergo inputTime/2 tiene que serlo

        var optimalT = inputTime / 2;

        int start = 1;
        int end = optimalT + 1;
        int optimizedTimeWithoutRecord = 0;

        while (true)
        {
            int time = (end - start) / 2;
            if ((long)time * (inputTime - time) < inputDistance)
            {
                optimizedTimeWithoutRecord = time;
                start = time;
                end += time;
            }
            else if (optimizedTimeWithoutRecord > 0)
            {
                return CountRecordsWithEarlyBreak(inputTime, inputDistance, optimizedTimeWithoutRecord);
            }
            else
            {
                end = time;
            }
        }

        throw new SolvingException();
    }

    private IEnumerable<(int Time, int Distance)> ParseInput()
    {
        var file = new ParsedFile(base.InputFilePath);

        var timesLine = file.NextLine();
        var distancesLine = file.NextLine();

        _ = timesLine.NextElement<string>();
        _ = distancesLine.NextElement<string>();

        if (timesLine.Count != distancesLine.Count)
        {
            throw new SolvingException();
        }

        while (!timesLine.Empty)
        {
            yield return (timesLine.NextElement<int>(), distancesLine.NextElement<int>());
        }

        if (!file.Empty)
        {
            throw new SolvingException();
        }
    }
}
