namespace AoC_2023;

public class Day_09 : BaseDay
{
    private readonly List<List<int>> _input;

    public Day_09()
    {
        _input = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        int result = 0;

        foreach (var input in _input)
        {
            var sequences = ReduceToZero(input);

            int prediction = 0;
            for (int i = 0; i < sequences.Count; i++)
            {
                prediction += sequences[i][^1];
            }

            result += prediction;
        }

        return new($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        int result = 0;

        foreach (var input in _input)
        {
            var sequences = ReduceToZero(input);

            int prediction = sequences[0][0];
            for (int i = 1; i < sequences.Count; i++)
            {
                prediction += (i % 2 == 0)
                    ? sequences[i][0]
                    : -sequences[i][0];
            }

            result += prediction;
        }

        return new($"{result}");
    }

    private static List<List<int>> ReduceToZero(List<int> input)
    {
        List<List<int>> sequences = new(input.Count) { new(input) };

        while (true)
        {
            var allZeros = true;
            List<int> lastSequence = sequences[^1];
            List<int> sequence = new(lastSequence.Count - 1);

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

        return sequences;
    }

    private IEnumerable<List<int>> ParseInput()
    {
        var file = new ParsedFile(InputFilePath);

        while (!file.Empty)
        {
            yield return file.NextLine().ToList<int>();
        }
    }
}
