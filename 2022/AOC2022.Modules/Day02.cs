using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2022.Modules;

public class Day02 : DayBase
{
    public override bool Completed => true;
    private enum Play
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }

    private enum Outcome
    {
        Loss = 0,
        Draw = 3,
        Win = 6,
    }
    
    public override dynamic Part1()
    {
        var rounds = get_input()
            .Select(MapInputToPlaysPart01)
            .ToList();
        var totalScore = 0;
        foreach (var round in rounds)
        {
            var outcome = DetermineRoundOutcome(round.opponent, round.me);
            var score = (int)outcome + (int)round.me;
            totalScore += score;
        }

        return new { totalScore };
    }

    public override dynamic Part2()
    {
        var rounds = get_input()
            .Select(MapInputToPlaysPart02)
            .ToList();
        var totalScore = 0;
        foreach (var round in rounds)
        {
            var outcome = DetermineRoundOutcome(round.opponent, round.me);
            var score = (int)outcome + (int)round.me;
            totalScore += score;
        }

        return new { totalScore };
    }

    private (Play opponent, Play me) MapInputToPlaysPart01(string roundInput)
    {
        var parts = roundInput.Split(' ', StringSplitOptions.TrimEntries);
        var opponent = inputToPlayMapPart01.FirstOrDefault(x => x.inputs.Contains(parts[0])).play;
        var me = inputToPlayMapPart01.FirstOrDefault(x => x.inputs.Contains(parts[1])).play;
        return (opponent, me);
    }
    
    private (Play opponent, Play me) MapInputToPlaysPart02(string roundInput)
    {
        var parts = roundInput.Split(' ', StringSplitOptions.TrimEntries);
        var opponent = inputToPlayMapPart01.FirstOrDefault(x => x.inputs.Contains(parts[0])).play;
        var desiredOutcome = inputToOutcomeMap.FirstOrDefault(x => x.input == parts[1]).outcome;
        var me = DeterminePlayFromDesiredOutcome(opponent, desiredOutcome);
        return (opponent, me);
    }

    private Play DeterminePlayFromDesiredOutcome(Play opponent, Outcome desiredOutcome)
    {
        var myPlay = opponent;
        switch (desiredOutcome)
        {
            case Outcome.Win:
                var winPlay = (int)opponent + 1;
                if (winPlay == 4)
                    winPlay = 1;
                myPlay = (Play)winPlay;
                break;
            case Outcome.Loss:
                var losePlay = (int)opponent - 1;
                if (losePlay == 0)
                    losePlay = 3;
                myPlay = (Play)losePlay;
                break;
        }

        return myPlay;
    }

    private List<(List<string> inputs, Play play)> inputToPlayMapPart01 = new List<(List<string> inputs, Play play)>
    {
        (new List<string>{ "A", "X" }, Play.Rock),
        (new List<string>{ "B", "Y" }, Play.Paper),
        (new List<string>{ "C", "Z" }, Play.Scissors),
    };

    private List<(string input, Outcome outcome)> inputToOutcomeMap = new List<(string input, Outcome outcome)>
    {
        ("X", Outcome.Loss),
        ("Y", Outcome.Draw),
        ("Z", Outcome.Win)
    };

    private Outcome DetermineRoundOutcome(Play opponent, Play me)
    {
        if (me == Play.Scissors && opponent == Play.Rock)
            return Outcome.Loss;
        
        if (me == Play.Rock && opponent == Play.Scissors)
            return Outcome.Win;
        
        if (opponent > me)
            return Outcome.Loss;

        if (me > opponent)
            return Outcome.Win;

        return Outcome.Draw;
    }
}