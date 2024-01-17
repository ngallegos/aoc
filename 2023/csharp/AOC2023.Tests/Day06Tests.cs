using Shouldly;

namespace AOC2023.Tests;

public class Day06Tests : TestBase
{
    protected override void SolvePart1_Sample()
    {
        var waysToWin = DetermineWaysToWin(get_sample());
        waysToWin.ShouldBe(288);
    }

    protected override void SolvePart2_Sample()
    {
        var waysToWin = DetermineWaysToWin(get_sample(), true);
        waysToWin.ShouldBe(71503);
    }

    protected override void SolvePart1_Actual()
    {
        var waysToWin = DetermineWaysToWin(get_input());
        waysToWin.ShouldBe(4403592L);
    }

    protected override void SolvePart2_Actual()
    {
        var waysToWin = DetermineWaysToWin(get_input(), true);
        waysToWin.ShouldBe(38017587L);
    }

    private static long DetermineWaysToWin(IEnumerable<string> input, bool correctKerning = false)
    {
        var stats = ParseInput(input, correctKerning);
        var allRacePossibilities =
            stats.Select(x => x.GetWinningPossibilities()).ToList();
        long waysToWin = 1;
        foreach (var racePossibilities in allRacePossibilities)
        {
            waysToWin *= racePossibilities.Count;
        }

        return waysToWin;
    }
    
    private static List<RaceStatistics> ParseInput(IEnumerable<string> input, bool correctKerning = false)
    {
        var inputs = input.ToArray();
        var times = inputs[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1)
            .Select(x => long.Parse(x.Trim())).ToList();
        
        var records = inputs[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1)
            .Select(x => long.Parse(x.Trim())).ToList();

        var stats = new List<RaceStatistics>();
        for (int i = 0; i < times.Count; i++)
            stats.Add(new RaceStatistics(times[i], records[i]));

        if (correctKerning)
            return new List<RaceStatistics> { CorrectKerning(stats) };
        return stats;
    }
    
    private static RaceStatistics CorrectKerning(List<RaceStatistics> input)
    {
        var time = long.Parse(string.Join("", input.Select(x => x.Time.ToString())));
        var record = long.Parse(string.Join("", input.Select(x => x.RecordDistance.ToString())));
        var stats = new RaceStatistics(time, record);
        return stats;
    }
    
    private class RaceStatistics
    {
        private readonly long _time;
        private readonly long _recordDistance;
        public long Time => _time;
        public long RecordDistance => _recordDistance;
        
        public RaceStatistics(long time, long recordDistance)
        {
            _time = time;
            _recordDistance = recordDistance;
        }
        
        public List<RacePossibility> GetWinningPossibilities()
        {
            
            var possibilities = new List<RacePossibility>();
            for (long i = 1; i < _time; i++)
            {
                var possibility = new RacePossibility(_time, i);
                if (possibility.TotalDistance > _recordDistance)
                    possibilities.Add(possibility);
            }

            return possibilities;
        }
    }
    
    private class RacePossibility
    {
        private readonly long _holdTime;
        private readonly long _totalDistance;
        public long TotalDistance => _totalDistance;
        
        public RacePossibility(long raceTime, long holdTime)
        {
            _holdTime = holdTime;
            _totalDistance = (raceTime - _holdTime) * holdTime;
        }
    }
}