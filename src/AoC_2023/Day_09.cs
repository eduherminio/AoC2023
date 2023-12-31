﻿namespace AoC_2023;

public class Day_09 : BaseDay
{
    private readonly List<List<List<int>>> _input;

    public Day_09()
    {
        _input = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        int result = 0;

        foreach (var sequences in _input)
        {
            foreach (var sequence in sequences)
            {
                result += sequence[^1];
            }
        }

        return new($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        int result = 0;

        foreach (var sequences in _input)
        {
            for (int i = 0; i < sequences.Count; i++)
            {
                result += (i % 2 == 1)
                    ? -sequences[i][0]
                    : sequences[i][0];
            }
        }

        return new($"{result}");
    }

    private static List<List<int>> ReduceToZero(List<int> input)
    {
        List<List<int>> sequences = new(input.Count) { input };

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

    private IEnumerable<List<List<int>>> ParseInput()
    {
        var file = new ParsedFile(InputFilePath);

        while (!file.Empty)
        {
            yield return ReduceToZero(file.NextLine().ToList<int>());
        }
    }
}
