using MoreLinq;
using SheepTools.Model;

namespace AoC_2023;
public class Day_13 : BaseDay
{
    private sealed record Point : IntPointWithValue<char>
    {
        public Point(char value, int x, int y) : base(x, y)
        {
            Value = value;
        }

        public bool IsRock => Value == '#';
        public bool IsAsh => Value == '.';
    }

    private readonly List<List<List<Point>>> _input;

    public Day_13()
    {
        _input = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        int result = 0;
        foreach (var pattern in _input)
        {
            int xReflection = -1;
            int yReflection = -1;

            // Vertical lines
            for (int x = 0; x < pattern[0].Count - 1; x++)
            {
                // Before the first half of the pattern
                if (x < (pattern[0].Count - 1) / 2)
                {
                    for (int y = 0; y < pattern.Count; ++y)
                    {
                        for (int i = 0; i <= x; ++i)
                        {
                            if (pattern[x - i][i].Value != pattern[y][x + i + 1].Value)
                            {
                                goto failure;
                            }
                        }
                    }

                    xReflection = x;
                    break;

                    failure:
                    xReflection = -1;
                }
                else // After the first half of the pattern
                {
                    for (int y = 0; y < pattern.Count; ++y)
                    {
                        for (int i = 1; i < pattern[y].Count - x; ++i)
                        {
                            if (pattern[y][x + i].Value != pattern[y][x - i + 1].Value)
                            {
                                goto failure;
                            }
                        }
                    }

                    xReflection = x;
                    break;

                    failure:
                    xReflection = -1;
                }
            }

            if (xReflection != -1)
            {
                result += xReflection + 1;
                continue;
            }

            // Horizontal lines
            for (int y = 0; y < pattern.Count - 1; y++)
            {
                // Above the first half of the pattern
                if (y < (pattern.Count - 1) / 2)
                {
                    for (int x = 0; x < pattern[y].Count; ++x)
                    {
                        for (int i = 0; i <= y; ++i)
                        {
                            if (pattern[y - i][x].Value != pattern[y + i + 1][x].Value)
                            {
                                goto failure;
                            }
                        }
                    }

                    yReflection = y;
                    break;

                    failure:
                    yReflection = -1;
                }
                else // Below the first half of the pattern
                {
                    for (int x = 0; x < pattern[y].Count; ++x)
                    {
                        for (int i = 1; i < pattern.Count - y; ++i)
                        {
                            if (pattern[y + i][x].Value != pattern[y - i + 1][x].Value)
                            {
                                goto failure;
                            }
                        }
                    }

                    yReflection = y;
                    break;

                    failure:
                    yReflection = -1;
                }
            }

            if (yReflection == -1)
            {
                throw new SolvingException();
            }

            result += 100 * (yReflection + 1);
        }

        return new($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        int result = 0;
        return new($"{result}");
    }

    private IEnumerable<List<List<Point>>> ParseInput()
    {
        foreach (var pattern in ParsedFile.ReadAllGroupsOfLines(InputFilePath))
        {
            var listOfList = new List<List<Point>>();

            for (int y = 0; y < pattern.Count; ++y)
            {
                var returnLine = new List<Point>(pattern.Count);
                for (int x = 0; x < pattern[y].Length; ++x)
                {
                    returnLine.Add(new(pattern[y][x], x, y));
                }

                listOfList.Add(returnLine);
            }

            yield return listOfList;
        }
    }
}
