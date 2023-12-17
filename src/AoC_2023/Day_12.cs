using MoreLinq;
using MoreLinq.Extensions;
using System.Collections.Specialized;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace AoC_2023;

public class Day_12 : BaseDay
{
    private sealed record Row(string Records, List<int> Conditions);

    private readonly List<Row> _input;

    public Day_12()
    {
        _input = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        int result = 0;

        foreach (Row row in _input)
        {
            List<int> unknownIndexes = new(row.Records.Length);

            var rowSpan = row.Records.AsSpan();
            for (int i = 0; i < rowSpan.Length; ++i)
            {
                if (rowSpan[i] == '?')
                {
                    unknownIndexes.Add(i);
                }
            }

            List<string> possibilities = ExtractPossibilities(row, unknownIndexes.Count);

            foreach (var possibility in possibilities)
            {
                var sb = new StringBuilder();
                var possibilityIndex = 0;
                for (int i = 0; i < row.Records.Length; ++i)
                {
                    if (unknownIndexes.Contains(i))
                    {
                        sb.Append(possibility[possibilityIndex++] == '0' ? '.' : '#');
                    }
                    else
                    {
                        sb.Append(row.Records[i]);
                    }
                }

                var finalRecord = sb.ToString();

                //Console.WriteLine(finalRecord);

                var lastBrokenGroup = 0;
                var conditionIndex = 0;

                bool isPossible = true;
                foreach (var spring in finalRecord)
                {
                    if (spring == '#')
                    {
                        lastBrokenGroup++;
                    }
                    else
                    {
                        if (conditionIndex >= row.Conditions.Count || lastBrokenGroup != row.Conditions[conditionIndex])
                        {
                            if (lastBrokenGroup > 0)
                            {
                                isPossible = false;
                                break;
                            }
                        }
                        else
                        {
                            ++conditionIndex;
                            lastBrokenGroup = 0;
                        }
                    }
                }

                if (conditionIndex == row.Conditions.Count - 1 && lastBrokenGroup > 0)
                {
                    if (lastBrokenGroup != row.Conditions[conditionIndex])
                    {
                        isPossible = false;
                    }

                    ++conditionIndex;
                }

                if (isPossible && conditionIndex == row.Conditions.Count && finalRecord.Count(ch => ch == '#') == row.Conditions.Sum())
                {
                    ++result;
                }
            }

            //Console.WriteLine(result);
        }

        return new($"{result}");
    }

    private static List<string> ExtractPossibilities(Row row, int unknownPositionsCount)
    {
        var possibilities = new List<string>(row.Records.Length * row.Records.Length);

        var maskSb = new StringBuilder();
        for (int i = 0; i < unknownPositionsCount; ++i)
        {
            maskSb.Append('1');
        }

        var mask = Convert.ToInt32(maskSb.ToString(), 2);

        //Console.WriteLine(unknownPositionsCount);
        //Console.WriteLine(mask);
        var maxCombinationsCountString = Convert.ToString(mask, 2);

        for (int i = 0; i <= mask; i++)
        {
            var sb = new StringBuilder(mask);

            var binaryString = Convert.ToString(i, 2);
            for (int j = binaryString.Length; j < maxCombinationsCountString.Length; ++j)
            {
                sb.Append('0');
            }

            sb.Append(binaryString);

            possibilities.Add(sb.ToString());
            //Console.WriteLine(possibilities.Last());
        }
        //Console.WriteLine();
        return possibilities;
    }

    public override ValueTask<string> Solve_2()
    {
        int result = 0;

        return new($"{result}");
    }

    private IEnumerable<Row> ParseInput()
    {
        char[] existingSeparator = [' ', ','];
        var file = new ParsedFile(base.InputFilePath, existingSeparator);

        while (!file.Empty)
        {
            var line = file.NextLine();

            var records = line.NextElement<string>();
            var conditions = new List<int>(line.Count);

            while (!line.Empty)
            {
                conditions.Add(line.NextElement<int>());
            }

            yield return new Row(records, conditions);
        }
    }
}
