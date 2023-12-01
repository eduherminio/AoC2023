namespace AoC_2023;

public class Day_01 : BaseDay
{
    private readonly string[] _input;

    private static readonly List<string> _digits =
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

    public override ValueTask<string> Solve_1() => new($"{Solve_1_NoLinq_OptimizedParsing()}");

    public override ValueTask<string> Solve_2() => new($"{Solve_2_Optimized()}");

    public int Solve_1_Original()
    {
        var result = 0;

        foreach (var item in _input)
        {
            var firstIndex = item.First(ch => int.TryParse(ch.ToString(), out _));
            var lastIndex = item.Last(ch => int.TryParse(ch.ToString(), out _));

            result += int.Parse($"{firstIndex}{lastIndex}");
        }

        return result;
    }

    public int Solve_1_NoLinq()
    {
        var result = 0;

        foreach (var item in _input)
        {
            int startIndex = 0;
            int endIndex = 0;

            bool startIndexFound = false;
            bool endIndexFound = false;

            for (int i = 0; i < item.Length; ++i)
            {
                if (!startIndexFound && int.TryParse(item[i].ToString(), out startIndex))
                {
                    startIndexFound = true;
                }

                if (!endIndexFound && int.TryParse(item[^(i + 1)].ToString(), out endIndex))
                {
                    endIndexFound = true;
                }
            }

            result += (startIndex * 10) + endIndex;
        }

        return result;
    }

    public int Solve_1_NoLinq_OptimizedParsing()
    {
        const char zero = '0';

        var result = 0;

        foreach (var item in _input)
        {
            int startIndex = 0;
            int endIndex = 0;

            bool startIndexFound = false;
            bool endIndexFound = false;

            for (int i = 0; i < item.Length; ++i)
            {
                if (!startIndexFound)
                {
                    startIndex = item[i] - zero;
                    if (startIndex >= 1 && startIndex <= 9)
                    {
                        startIndexFound = true;
                    }
                }

                if (!endIndexFound)
                {
                    endIndex = item[i] - zero;
                    if (endIndex >= 1 && endIndex <= 9)
                    {
                        endIndexFound = true;
                    }
                }
            }
            result += (startIndex * 10) + endIndex;
        }

        return result;
    }

    public int Solve_2_Original()
    {
        var result = 0;

        foreach (var item in _input)
        {
            result += FindResult(item);
        }

        return result;

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

    public int Solve_2_Optimized()
    {
        var result = 0;

        foreach (var item in _input)
        {
            result += FindResult(item);
        }

        return result;

        // To avoid stackalloc in the loop
        static int FindResult(string item)
        {
            const char zero = '0';

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
                    startIndex = item[i] - zero;
                    if (startIndex >= 1 && startIndex <= 9)
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
                    endIndex = item[^(i + 1)] - zero;
                    if (endIndex >= 1 && endIndex <= 9)
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
                    var lengthLimit = Math.Clamp(trimmedStartWord.Length - j, 1, 5);    // Being 5 the max length of the diigts
                    for (int length = 3; length <= lengthLimit; ++length)               // Being 3 the min length of the digits
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
                    var lengthLimit = Math.Clamp(trimmedEndWord.Length - j, 1, 5);  // Being 5 the max length of the diigts
                    for (int length = 3; length <= lengthLimit; ++length)           // Being 3 the min length of the digits
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
