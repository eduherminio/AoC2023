﻿using System.ComponentModel.DataAnnotations;
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

    private readonly Regex _gameRegex = GameRegex();
    private readonly Regex _blueRegex = BlueRegex();
    private readonly Regex _redRegex = RedRegex();
    private readonly Regex _greenRegex = GreenRegex();

    private sealed record CubeSet(int Red, int Green, int Blue);

    private sealed record Game(int Id, List<CubeSet> CubeSets);

    private readonly List<Game> _input;

    public Day_02()
    {
        _input = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        const int maxRedCubes = 12;
        const int maxGreenCubes = 13;
        const int maxBlueCubes = 14;

        var possibleGames = _input.Where(game =>
            game.CubeSets.TrueForAll(cubeSet =>
                cubeSet.Red <= maxRedCubes
                && cubeSet.Blue <= maxBlueCubes
                && cubeSet.Green <= maxGreenCubes));

        var result = possibleGames.Sum(g => g.Id);

        return new($"{result}");
    }

    public override ValueTask<string> Solve_2()
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

        return new($"{result}");
    }

    private IEnumerable<Game> ParseInput()
    {
        var file = new ParsedFile(InputFilePath, new[] { ';', ':' });

        while (!file.Empty)
        {
            var line = file.NextLine();

            var firstItem = line.NextElement<string>();
            var gameNumber = int.Parse(_gameRegex.Match(firstItem).Value);
            var cubeSetList = new List<CubeSet>(128);

            while (!line.Empty)
            {
                var setString = line.NextElement<string>();

                if (!int.TryParse(_redRegex.Match(setString).ValueSpan, out var redCount))
                {
                    redCount = 0;
                }

                if (!int.TryParse(_greenRegex.Match(setString).ValueSpan, out var greenCount))
                {
                    greenCount = 0;
                }

                if (!int.TryParse(_blueRegex.Match(setString).ValueSpan, out var blueCount))
                {
                    blueCount = 0;
                }

                cubeSetList.Add(new CubeSet(redCount, greenCount, blueCount));
            }

            yield return new Game(gameNumber, cubeSetList);
        }
    }
}