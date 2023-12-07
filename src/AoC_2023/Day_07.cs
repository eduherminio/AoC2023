namespace AoC_2023;

public class Day_07 : BaseDay
{
    private sealed record Hand
    {
        private int _strengthPart1 = 0;

        public int StrengthPart1
        {
            get
            {
                if (_strengthPart1 == 0)
                {
                    _strengthPart1 = CalculateStrength_Part1(Cards);
                }

                return _strengthPart1;
            }
        }

        /// <summary>
        /// Numeral cards not included, use <see cref="char"/> - '0' for that
        /// </summary>
        private static readonly IReadOnlyDictionary<char, int> _cardValuesPart1 = new Dictionary<char, int>
        {
            ['T'] = 10,
            ['J'] = 11,
            ['Q'] = 12,
            ['K'] = 13,
            ['A'] = 14,
        };

        private int _strengthPart2 = 0;

        public int StrengthPart2
        {
            get
            {
                if (_strengthPart2 == 0)
                {
                    _strengthPart2 = CalculateStrength_Part2(Cards);
                }

                return _strengthPart2;
            }
        }

        /// <summary>
        /// Numeral cards not included, use <see cref="char"/> - '0' for that
        /// </summary>
        private static readonly IReadOnlyDictionary<char, int> _cardValuesPart2 = new Dictionary<char, int>
        {
            ['T'] = 10,
            ['J'] = 1,
            ['Q'] = 12,
            ['K'] = 13,
            ['A'] = 14,
        };

        public string Cards { get; }

        public int Bid { get; }

        public Hand(string cards, int bid)
        {
            Bid = bid;
            Cards = cards;
        }

        private static int CalculateStrength_Part1(string cards)
        {
            int mainScore = MainScore_Part1(cards);

            int secondaryScore = SecondaryScore_Part1(cards);

            if (mainScore <= secondaryScore)
            {
                throw new SolvingException();
            }

            return mainScore + secondaryScore;
        }

        private static int MainScore_Part1(string cards)
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

        private static int SecondaryScore_Part1(string cards)
        {
            return cards
                .Select((ch, index) =>
                    (int)Math.Pow(15, cards.Length - index - 1)
                    * (_cardValuesPart1.TryGetValue(ch, out var score)
                        ? score
                        : ch - '0'))
                .Sum();
        }

        private static int CalculateStrength_Part2(string cards)
        {
            int mainScore = MainScore_Part2(cards);

            int secondaryScore = SecondaryScore_Part2(cards);

            if (mainScore <= secondaryScore)
            {
                throw new SolvingException();
            }

            return mainScore + secondaryScore;
        }

        private static int MainScore_Part2(string cards)
        {
            var cardsWithoutJoker = cards.Replace("J", "");
            var jokerCount = cards.Length - cardsWithoutJoker.Length;

            var groups = cardsWithoutJoker
                .GroupBy(ch => ch)
                .OrderByDescending(g => g.Count())
                .ToList();

            var firstGroupCount = (groups.FirstOrDefault()?.Count() ?? 0) + jokerCount;

            return groups.Count switch
            {
                0 or 1 => 100_000_000,                          // Five of a kind, 0 for all jokers
                2 when firstGroupCount == 4 => 90_000_000,      // Four of a kind
                2 when firstGroupCount == 3 => 80_000_000,      // Full house
                3 when firstGroupCount == 3 => 70_000_000,      // Three of a kind
                3 when firstGroupCount == 2 => 60_000_000,      // Two pair
                4 => 50_000_000,                                // Pair
                5 => 40_000_000,                                // High card
                _ => throw new()
            };
        }

        private static int SecondaryScore_Part2(string cards)
        {
            return cards
                .Select((ch, index) =>
                    (int)Math.Pow(15, cards.Length - index - 1)
                    * (_cardValuesPart2.TryGetValue(ch, out var score)
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
        foreach (var hand in _input.OrderBy(h => h.StrengthPart1))
        {
            result += hand.Bid * index++;
        }

        return new($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        long result = 0;

        long index = 1;
        foreach (var hand in _input.OrderBy(h => h.StrengthPart2))
        {
            result += hand.Bid * index++;
        }

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
