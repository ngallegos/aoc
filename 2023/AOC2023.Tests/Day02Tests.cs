using System.Text.RegularExpressions;
using Shouldly;

namespace AOC2023.Tests;

public class Day02Tests : TestBase
{
    [Test]
    public override void Part1_Sample()
    {
        // Arrange
        var games = get_sample()
            .Select(GetGameStats).ToList();
        var expectedSum = 8;
        
        // Act
        var possibleGames = games
            .Where(x => x.MaxRed <= _maxRed && x.MaxGreen <= _maxGreen && x.MaxBlue <= _maxBlue).ToList();
        var actualSum = possibleGames.Sum(x => x.GameNumber);

        // Assert
        actualSum.ShouldBe(expectedSum);
    }
    
    
    [Test]
    public override void Part1_Actual()
    {
        // Arrange
        var games = get_input()
            .Select(GetGameStats).ToList();
        var expectedSum = 2283;
        
        // Act
        var possibleGames = games
            .Where(x => x.MaxRed <= _maxRed && x.MaxGreen <= _maxGreen && x.MaxBlue <= _maxBlue).ToList();
        var actualSum = possibleGames.Sum(x => x.GameNumber);

        // Assert
        actualSum.ShouldBe(expectedSum);
    }
    
    [Test]
    public override void Part2_Sample()
    {
        // Arrange
        var games = get_sample()
            .Select(GetGameStats).ToList();
        var expectedSum = 2286;
        
        // Act
        var actualSum = games.Sum(x => x.MinPower);

        // Assert
        actualSum.ShouldBe(expectedSum);
    }

    
    [Test]
    public override void Part2_Actual()
    {
        // Arrange
        var games = get_input()
            .Select(GetGameStats).ToList();
        var expectedSum = 78669;
        
        // Act
        var actualSum = games.Sum(x => x.MinPower);

        // Assert
        actualSum.ShouldBe(expectedSum);
    }
    
    private int _maxRed = 12;
    private int _maxGreen = 13;
    private int _maxBlue = 14;

    private Game GetGameStats(string gameInput)
    {
        var gameRegex = new Regex(@"Game (?<gameNumber>\d+):(?<handfuls>.*)");
        var cubesRegex = new Regex(@"(?<cubes>\d+) (?<color>(blue|green|red))");
        var match = gameRegex.Match(gameInput);
        var game = new Game
        {
            GameNumber = int.Parse(match.Groups["gameNumber"].Value)
        };

        var handfuls = match.Groups["handfuls"].Value.Split(';');
        foreach (var handful in handfuls)
        {
            var cubeMatches = cubesRegex.Matches(handful);
            foreach (Match cubeMatch in cubeMatches)
            {
                switch (cubeMatch.Groups["color"].Value)
                {
                    case "red":
                        game.MaxRed = Math.Max(game.MaxRed, int.Parse(cubeMatch.Groups["cubes"].Value));
                        break;
                    case "green":
                        game.MaxGreen = Math.Max(game.MaxGreen, int.Parse(cubeMatch.Groups["cubes"].Value));
                        break;
                    case "blue":
                        game.MaxBlue = Math.Max(game.MaxBlue, int.Parse(cubeMatch.Groups["cubes"].Value));
                        break;
                }
            }
        }
        return game;
    }
    
    public class Game
    {
        public int GameNumber { get; set; }
        public int MaxRed { get; set; }
        public int MaxGreen { get; set; }
        public int MaxBlue { get; set; }
        public int MinPower => MaxRed * MaxBlue * MaxGreen;
    }
}