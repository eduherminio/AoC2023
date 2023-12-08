using System.Text.RegularExpressions;

namespace AoC_2023;

public partial class Day_02 : BaseDay
{
    [GeneratedRegex(@"(?<=Game\s)\d*")]
    private static partial Regex GameRegex();

    [GeneratedRegex(@"\d*(?=\sblue)")]
    private static partial Regex BlueRegex();

    [GeneratedRegex(@"\d*(?=\sred)")]
    private static partial Regex RedRegex();

    [GeneratedRegex(@"\d*(?=\sgreen)")]
    private static partial Regex GreenRegex();

    private sealed record CubeSet(int Red, int Green, int Blue);

    private sealed record Game(int Id, List<CubeSet> CubeSets);

    private readonly List<Game> _input;

    public Day_02()
    {
        _input = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_NoLinq()}");

    public override ValueTask<string> Solve_2() => new($"{Solve_2_Original()}");

    public int Solve_1_Original()
    {
        const int maxRedCubes = 12;
        const int maxGreenCubes = 13;
        const int maxBlueCubes = 14;

        return _input
            .Where(game =>
                game.CubeSets.TrueForAll(cubeSet =>
                    cubeSet.Red <= maxRedCubes
                    && cubeSet.Blue <= maxBlueCubes
                    && cubeSet.Green <= maxGreenCubes))
            .Sum(g => g.Id);
    }

    public int Solve_1_NoLinq()
    {
        const int maxRedCubes = 12;
        const int maxGreenCubes = 13;
        const int maxBlueCubes = 14;

        int result = 0;

        foreach (var game in _input)
        {
            bool isValid = true;
            foreach (var cubeSet in game.CubeSets)
            {
                if (cubeSet.Red > maxRedCubes || cubeSet.Blue > maxBlueCubes || cubeSet.Green > maxGreenCubes)
                {
                    isValid = false;
                    break;
                }
            }

            if (isValid)
            {
                result += game.Id;
            }
        }

        return result;
    }

    public int Solve_2_Linq()
    {
        return _input.Sum(game =>
            game.CubeSets.Max(cubeSet => cubeSet.Red)
            * game.CubeSets.Max(cubeSet => cubeSet.Green)
            * game.CubeSets.Max(cubeSet => cubeSet.Blue));
    }

    public int Solve_2_Original()
    {
        int result = 0;

        foreach (var game in _input)
        {
            int maxRed = 0, maxGreen = 0, maxBlue = 0;

            foreach (var cubeSet in game.CubeSets)
            {
                if (cubeSet.Red > maxRed)
                {
                    maxRed = cubeSet.Red;
                }
                if (cubeSet.Blue > maxBlue)
                {
                    maxBlue = cubeSet.Blue;
                }
                if (cubeSet.Green > maxGreen)
                {
                    maxGreen = cubeSet.Green;
                }
            }

            result += maxRed * maxGreen * maxBlue;
        }

        return result;
    }

    private IEnumerable<Game> ParseInput()
    {
        char[] existingSeparator = [';', ':'];
        var file = new ParsedFile(base.InputFilePath, existingSeparator);

        while (!file.Empty)
        {
            var line = file.NextLine();

            var firstItem = line.NextElement<string>();
            var gameNumber = int.Parse(GameRegex().Match(firstItem).Value);
            var cubeSetList = new List<CubeSet>(128);

            while (!line.Empty)
            {
                var setString = line.NextElement<string>();

                if (!int.TryParse(RedRegex().Match(setString).ValueSpan, out var redCount))
                {
                    redCount = 0;
                }

                if (!int.TryParse(GreenRegex().Match(setString).ValueSpan, out var greenCount))
                {
                    greenCount = 0;
                }

                if (!int.TryParse(BlueRegex().Match(setString).ValueSpan, out var blueCount))
                {
                    blueCount = 0;
                }

                cubeSetList.Add(new CubeSet(redCount, greenCount, blueCount));
            }

            yield return new Game(gameNumber, cubeSetList);
        }
    }
}
