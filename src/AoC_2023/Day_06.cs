using Microsoft.VisualBasic;

namespace AoC_2023;

public class Day_06 : BaseDay
{
    private readonly List<(int Time, int Distance)> _input;

    public Day_06()
    {
        _input = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_Original()}");

    public override ValueTask<string> Solve_2() => new($"{Solve_2_Original()}");

    public long Solve_1_Original()
    {
        long result = 1;
        foreach (var (inputTime, inputDistance) in _input)
        {
            int occurrences = 0;
            for (int t = 0; t < inputTime; ++t)
            {
                var speed = t;
                var time = inputTime - t;

                var distance = speed * time;

                if (distance > inputDistance)
                {
                    ++occurrences;
                }
            }

            result *= occurrences;
        }

        return result;
    }

    public int Solve_2_Original()
    {
        int result = 0;


        return result;
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
