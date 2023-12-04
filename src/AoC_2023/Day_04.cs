namespace AoC_2023;

public class Day_04 : BaseDay
{
    private sealed record Card(int Id, List<int> WinningNumbers, List<int> CardNumbers);

    private readonly List<Card> _input;

    public Day_04()
    {
        _input = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_Original()}");

    public override ValueTask<string> Solve_2() => new($"{Solve_2_Original()}");

    public int Solve_1_Original()
    {
        return _input.Sum(card => (int)Math.Pow(2, card.CardNumbers.Intersect(card.WinningNumbers).Count() - 1));
    }

    public int Solve_2_Original()
    {
        Dictionary<Card, int> cards = _input.ToDictionary(card => card, _ => 1);

        for (int dictIndex = 0; dictIndex < cards.Count; ++dictIndex)
        {
            var card = cards.ElementAt(dictIndex);

            var cardsWon = card.Key.WinningNumbers.Intersect(card.Key.CardNumbers).Count();
            for (int i = card.Key.Id; i < card.Key.Id + cardsWon; i++)
            {
                cards[_input[i]] += card.Value;
            }
        }

        return cards.Values.Sum();
    }

    private IEnumerable<Card> ParseInput()
    {
        char[] separators = [' ', ':'];
        var file = new ParsedFile(InputFilePath, separators);

        while (!file.Empty)
        {
            var line = file.NextLine();
            _ = line.NextElement<string>(); // Card
            var card = new Card(line.NextElement<int>(), new(line.Count / 2), new(line.Count / 2));

            var currentCollection = card.WinningNumbers;
            while (!line.Empty)
            {
                if (!int.TryParse(line.NextElement<string>(), out var n))
                {
                    currentCollection = card.CardNumbers;
                    n = line.NextElement<int>();
                }

                currentCollection.Add(n);
            }

            yield return card;
        }
    }
}
