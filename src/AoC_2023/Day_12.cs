using MoreLinq;
using MoreLinq.Extensions;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Diagnostics;
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
        int result = SolveBackwards(_input);

        return new($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        //return new("");
        var newInput = _input.ConvertAll(row =>
        {
            var newRow = row with
            {
                Records = $"{row.Records}?{row.Records}?{row.Records}?{row.Records}?{row.Records}",
                Conditions = [.. row.Conditions.ToList()]
            };

            newRow.Conditions.AddRange(row.Conditions);
            newRow.Conditions.AddRange(row.Conditions);
            newRow.Conditions.AddRange(row.Conditions);
            newRow.Conditions.AddRange(row.Conditions);

            return newRow;
        });

        int result = SolveBackwards(newInput);

        return new($"{result}");
    }

    private static int Solve(List<Row> input)
    {
        int result = 0;

        foreach (Row row in input)
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
            HashSet<string> visitedPossibilities = new(possibilities.Count);

            for (int possibilityIndex = 0; possibilityIndex < possibilities.Count; possibilityIndex++)
            {
                string possibility = possibilities[possibilityIndex];
                if (!visitedPossibilities.Add(possibility))
                {
                    continue;
                }

                var sb = new StringBuilder();
                var localPossibilityIndex = 0;
                for (int i = 0; i < row.Records.Length; ++i)
                {
                    if (unknownIndexes.Contains(i))
                    {
                        sb.Append(possibility[localPossibilityIndex++] == '0' ? '.' : '#');
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
                for (int finalRecordIndex = 0; finalRecordIndex < finalRecord.Length; finalRecordIndex++)
                {
                    char spring = finalRecord[finalRecordIndex];

                    if (spring == '#')
                    {
                        lastBrokenGroup++;
                    }
                    else
                    {
                        if (conditionIndex >= row.Conditions.Count)
                        {
                            if (lastBrokenGroup > 0)
                            {
                                isPossible = false;

                                // Old approach
                                var unknownPositionsBefore = row.Records.Where((ch, index) => index < finalRecordIndex && ch == '?').Count();

                                if (unknownPositionsBefore < possibility.Length - 1)
                                {
                                    var discardedPossibility = possibility[..(unknownPositionsBefore + 1)];
                                    possibilities.RemoveAll(x => x.StartsWith(discardedPossibility));
                                    possibilityIndex = -1;
                                }

                                break;
                            }
                        }
                        else if (lastBrokenGroup != row.Conditions[conditionIndex])
                        {
                            if (lastBrokenGroup > 0)
                            {
                                isPossible = false;

                                //List<int> possibilityIndexesToRemove = new(lastBrokenGroup);

                                //var patternIndex = finalRecordIndex;
                                //while (patternIndex >= 0 && patternIndex >= finalRecordIndex - 1 - lastBrokenGroup)
                                //{
                                //    var positionIndex = unknownIndexes.IndexOf(patternIndex);
                                //    if (positionIndex != -1)
                                //    {
                                //        possibilityIndexesToRemove.Add(positionIndex);
                                //    }

                                //    --patternIndex;
                                //    if (patternIndex < 0 || row.Records[patternIndex] == '.')
                                //    {
                                //        break;
                                //    }
                                //}
                                //possibilities.RemoveAll(combination => possibilityIndexesToRemove.TrueForAll(item => combination[item] == possibility[item]));
                                //possibilityIndex = -1;

                                // Old approach
                                var unknownPositionsBefore = row.Records.Where((ch, index) => index <= finalRecordIndex && ch == '?').Count();

                                if (unknownPositionsBefore < possibility.Length - 1)
                                {
                                    var discardedPossibility = possibility[..(unknownPositionsBefore + 1)];
                                    possibilities.RemoveAll(x => x.StartsWith(discardedPossibility));
                                    possibilityIndex = -1;
                                }

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
                    possibilities.Remove(possibility);
                    --possibilityIndex;
                }
            }

            Console.WriteLine(result);
        }

        return result;
    }

    private static int SolveBackwards(List<Row> input)
    {
        int result = 0;

        foreach (Row row in input)
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
            Console.WriteLine($"Possibilities: {possibilities.Count}");

            HashSet<string> visitedPossibilities = new(possibilities.Count);

            for (int possibilityIndex = 0; possibilityIndex < possibilities.Count; possibilityIndex++)
            {
                string possibility = possibilities[possibilityIndex];
                if (!visitedPossibilities.Add(possibility))
                {
                    continue;
                }

                var sb = new StringBuilder();
                var localPossibilityIndex = 0;
                for (int i = 0; i < row.Records.Length; ++i)
                {
                    if (unknownIndexes.Contains(i))
                    {
                        sb.Append(possibility[localPossibilityIndex++] == '0' ? '.' : '#');
                    }
                    else
                    {
                        sb.Append(row.Records[i]);
                    }
                }

                var finalRecord = sb.ToString();

                //Console.WriteLine(finalRecord);

                var lastBrokenGroup = 0;
                var conditionIndex = row.Conditions.Count - 1;

                bool isPossible = true;
                for (int finalRecordIndex = finalRecord.Length -1; finalRecordIndex >= 0; finalRecordIndex--)
                {
                    char spring = finalRecord[finalRecordIndex];

                    if (spring == '#')
                    {
                        lastBrokenGroup++;
                    }
                    else
                    {
                        if (conditionIndex < 0)
                        {
                            if (lastBrokenGroup > 0)
                            {
                                isPossible = false;

                                // Old approach
                                var unknownPositionsBefore = row.Records.Where((ch, index) => index > finalRecordIndex && ch == '?').Count();

                                if (unknownPositionsBefore < possibility.Length - 1)
                                {
                                    var discardedPossibility = possibility[^(unknownPositionsBefore + 2)..];
                                    //possibilities.RemoveAll(x => x.EndsWith(discardedPossibility));
                                    possibilityIndex = -1;
                                }

                                break;
                            }
                        }
                        else if (lastBrokenGroup != row.Conditions[conditionIndex])
                        {
                            if (lastBrokenGroup > 0)
                            {
                                isPossible = false;

                                //List<int> possibilityIndexesToRemove = new(lastBrokenGroup);

                                //var patternIndex = finalRecordIndex;
                                //while (patternIndex >= 0 && patternIndex >= finalRecordIndex - 1 - lastBrokenGroup)
                                //{
                                //    var positionIndex = unknownIndexes.IndexOf(patternIndex);
                                //    if (positionIndex != -1)
                                //    {
                                //        possibilityIndexesToRemove.Add(positionIndex);
                                //    }

                                //    --patternIndex;
                                //    if (patternIndex < 0 || row.Records[patternIndex] == '.')
                                //    {
                                //        break;
                                //    }
                                //}
                                //possibilities.RemoveAll(combination => possibilityIndexesToRemove.TrueForAll(item => combination[item] == possibility[item]));
                                //possibilityIndex = -1;

                                // Old approach
                                var unknownPositionsBefore = row.Records.Where((ch, index) => index > finalRecordIndex && ch == '?').Count();

                                if (unknownPositionsBefore < possibility.Length - 1)
                                {
                                    var discardedPossibility = possibility[^(unknownPositionsBefore + 2)..];
                                    possibilities.RemoveAll(x => x.EndsWith(discardedPossibility));
                                    possibilityIndex = -1;
                                }

                                break;
                            }
                        }
                        else
                        {
                            --conditionIndex;
                            lastBrokenGroup = 0;
                        }
                    }
                }

                if (conditionIndex == 0 && lastBrokenGroup > 0)
                {
                    if (lastBrokenGroup != row.Conditions[conditionIndex])
                    {
                        isPossible = false;
                    }

                    --conditionIndex;
                }

                if (isPossible && conditionIndex == -1 && finalRecord.Count(ch => ch == '#') == row.Conditions.Sum())
                {
                    ++result;
                    possibilities.Remove(possibility);
                    --possibilityIndex;
                }
            }

            Console.WriteLine(result);
        }

        return result;
    }

    private static List<string> ExtractPossibilities(Row row, int unknownPositionsCount)
    {
        var possibilities = new List<string>(row.Records.Length * row.Records.Length);
        //var possibilities = new ConcurrentBag<string>();

        var maskSb = new StringBuilder(unknownPositionsCount);
        for (int i = 0; i < unknownPositionsCount; ++i)
        {
            maskSb.Append('1');
        }

        long mask = Convert.ToInt64(maskSb.ToString(), 2);

        var maxCombinationsCountString = Convert.ToString(mask, 2);

        var expectedOnesCount = row.Conditions.Sum() - row.Records.Count(ch => ch == '#');

        //Parallel.For(0, mask, n =>
        for (long n = 0; n <= mask; n++)
        {
            var binaryString = Convert.ToString(n, 2);
            if (binaryString.Count(ch => ch == '1') != expectedOnesCount)
            {
                continue;
                //return;
            }

            var sb = new StringBuilder(maxCombinationsCountString.Length);
            for (int j = binaryString.Length; j < maxCombinationsCountString.Length; ++j)
            {
                sb.Append('0');
            }
            sb.Append(binaryString);
            var candidate = sb.ToString();

            char previousChar = 'ñ';
            int groupCount = 0;
            int possIndex = 0;
            for (int j = 0; j < row.Records.Length; ++j)
            {
                var charToAdd = row.Records[j] == '?' ? candidate[possIndex++] : row.Records[j];
                if ((charToAdd == '#' || charToAdd == '1') && previousChar != '#' && previousChar != '1')
                {
                    ++groupCount;
                }

                previousChar = charToAdd;
            }

            if (groupCount != row.Conditions.Count)
            {
                continue;
                //return;
            }

            possibilities.Add(candidate);
        }
        //);

        return possibilities;
        //return [..possibilities];
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
