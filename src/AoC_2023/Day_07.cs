namespace AoC_2023;

public class Day_07 : BaseDay
{
    private sealed record Hand
    {
        /// <summary>
        /// Numeral cards not included, use <see cref="char"/> - '0' for that
        /// </summary>
        private static readonly IReadOnlyDictionary<char, int> _cardValues = new Dictionary<char, int>
        {
            ['T'] = 10,
            ['J'] = 11,
            ['Q'] = 12,
            ['K'] = 13,
            ['A'] = 14,
        };

        public string Cards { get; }
        public int Bid { get; }

        public int Strength { get; }

        public Hand(string cards, int bid)
        {
            Bid = bid;
            Cards = cards;

            int mainScore = MainScore(cards);

            int secondaryScore = SecondaryScore(cards);

            if (mainScore <= secondaryScore)
            {
                throw new SolvingException();
            }

            Strength = mainScore + secondaryScore;
        }

        private static int MainScore(string cards)
        {
            var groups = cards
                .GroupBy(ch => ch)
                .OrderByDescending(g => g.Count())
                .ToList();

            var firstGroupCount = groups[0].Count();

            return groups.Count switch
            {
                1 => 100_000_000,                                // Five of a kind
                2 when firstGroupCount == 4 => 90_000_000,       // Four of a kind
                2 when firstGroupCount == 3 => 80_000_000,       // Full house
                3 when firstGroupCount == 3 => 70_000_000,       // Three of a kind
                3 when firstGroupCount == 2 => 60_000_000,       // Two pair
                4 => 50_000_000,                                 // Pair
                5 => 40_000_000,                                 // High card
                _ => throw new()
            };
        }

        private static int SecondaryScore(string cards)
        {
            return cards
                .Select((ch, index) =>
                    (int)Math.Pow(15, cards.Length - index - 1)
                    * (_cardValues.TryGetValue(ch, out var score)
                        ? score
                        : ch - '0'))
                .Sum();
        }
    }

    private readonly List<Hand> _input;

    public Day_07()
    {
        _input = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        long result = 0;

        long index = 1;
        foreach(var hand in _input.OrderBy(h => h.Strength))
        {
            result += hand.Bid * index++;
        }

        return new($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        var result = 0;

        return new($"{result}");
    }

    private IEnumerable<Hand> ParseInput()
    {
        var parsedFile = new ParsedFile(InputFilePath);

        while (!parsedFile.Empty)
        {
            var line = parsedFile.NextLine();

            yield return new(line.NextElement<string>(), line.NextElement<int>());
        }
    }
}
