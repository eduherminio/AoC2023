using MoreLinq;

namespace AoC_2023;

public class Day_09 : BaseDay
{
    private readonly List<List<long>> _input;

    public Day_09()
    {
        _input = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_Original()}");

    public override ValueTask<string> Solve_2() => new($"{Solve_2_Original()}");

    public long Solve_1_Original()
    {
        long result = 0;

        foreach (var input in _input)
        {
            List<List<long>> sequences = new(input.Count) { new(input) };

            while (true)
            {
                var allZeros = true;
                List<long> lastSequence = sequences[^1];
                List<long> sequence = new(lastSequence.Count - 1);

                for (int i = 0; i < lastSequence.Count - 1; i++)
                {
                    var n = lastSequence[i + 1] - lastSequence[i];
                    if (n != 0)
                    {
                        allZeros = false;
                    }

                    sequence.Add(n);
                }

                if (allZeros)
                {
                    break;
                }

                sequences.Add(sequence);
            }

            long prediction = 0;
            for (int i = 0; i < sequences.Count; i++)
            {
                prediction += sequences[i][^1];
            }

            result += prediction;
        }

        return result;
    }

    public long Solve_2_Original()
    {
        var result = 0;

        return result;
    }

    private IEnumerable<List<long>> ParseInput()
    {
        var file = new ParsedFile(InputFilePath);
        while (!file.Empty)
        {
            yield return file.NextLine().ToList<long>();
        }
    }
}
