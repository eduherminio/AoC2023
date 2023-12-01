// using MoreLinq;

namespace AoC_2023;

public class Day_01 : BaseDay
{
    private readonly string[] _input;

    private readonly List<string> _digits =
    [
        "zero",
        "one",
        "two",
        "three",
        "four",
        "five",
        "six",
        "seven",
        "eight",
        "nine"
    ];

    public Day_01()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var result = 0;

        foreach (var item in _input)
        {
            var firstIndex = item.First(ch => int.TryParse(ch.ToString(), out _));
            var lastIndex = item.Last(ch => int.TryParse(ch.ToString(), out _));

            result += int.Parse($"{firstIndex}{lastIndex}");
        }

        return new($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        var result = 0;

        foreach (var item in _input)
        {
            result += FindResult(item);
        }

        return new($"{result}");

        // To avoid stackalloc in the loop
        int FindResult(string item)
        {
            int startIndex = 0;
            int endIndex = 0;

            bool startIndexFound = false;
            bool endIndexFound = false;

            Span<char> startWord = stackalloc char[item.Length];
            Span<char> endWord = stackalloc char[item.Length];

            for (int i = 0; i < item.Length; ++i)
            {
                if (!startIndexFound)
                {
                    if (int.TryParse(item[i].ToString(), out startIndex))
                    {
                        startIndexFound = true;
                    }
                    else
                    {
                        FindStartIndex(item, ref startIndex, ref startIndexFound, startWord, i);
                    }
                }

                if (!endIndexFound)
                {
                    if (int.TryParse(item[^(i + 1)].ToString(), out endIndex))
                    {
                        endIndexFound = true;
                    }
                    else
                    {
                        FindEndIndex(item, ref endIndex, ref endIndexFound, endWord, i);
                    }
                }
            }

            return (startIndex * 10) + endIndex;

            // To avoid stackalloc in the loop
            void FindStartIndex(string item, ref int firstIndex, ref bool startIndexFound, Span<char> startWord, int i)
            {
                startWord[i] = item[i];

                Span<char> trimmedStartWord = stackalloc char[item.Length];

                startWord.CopyTo(trimmedStartWord);
                trimmedStartWord = trimmedStartWord.TrimEnd('\0');

                for (int j = 0; j < trimmedStartWord.Length; ++j)
                {
                    for (int length = 1; length <= trimmedStartWord.Length - j; ++length)
                    {
                        var wordToTry = trimmedStartWord.Slice(j, length);

                        firstIndex = _digits.IndexOf(wordToTry.ToString());
                        if (firstIndex != -1)
                        {
                            startIndexFound = true;
                            break;
                        }
                    }

                    if (startIndexFound)
                    {
                        break;
                    }
                }
            }

            // To avoid stackalloc in the loop
            void FindEndIndex(string item, ref int lastIndex, ref bool endIndexFound, Span<char> endWord, int i)
            {
                endWord[i] = item[^(i + 1)];

                Span<char> trimmedEndWord = stackalloc char[item.Length];

                endWord.CopyTo(trimmedEndWord);
                trimmedEndWord = trimmedEndWord.TrimEnd('\0');
                trimmedEndWord.Reverse();

                for (int j = 0; j < trimmedEndWord.Length; ++j)
                {
                    for (int length = 1; length <= trimmedEndWord.Length - j; ++length)
                    {
                        var wordToTry = trimmedEndWord.Slice(j, length);

                        lastIndex = _digits.IndexOf(wordToTry.ToString());
                        if (lastIndex != -1)
                        {
                            endIndexFound = true;
                            break;
                        }
                    }

                    if (endIndexFound)
                    {
                        break;
                    }
                }
            }
        }
    }
}
