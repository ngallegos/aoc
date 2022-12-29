using System.Collections.Generic;
using System.Linq;

namespace AOC2022.Modules;

public class Day17 : DayBase
{
    public override bool Ignore { get; }
    public override dynamic Part1()
    {
        var gasJets = get_sample().First();
        var rockQueue = getInitialRockQueue();
        var jetIndex = 0;
        var gasJetsCount = gasJets.Length;
        var tower = new Stack<string>();
        for (int r = 0; r < 2022; r++)
        {
            jetIndex = DropRock(tower, rockQueue, gasJets, jetIndex, gasJetsCount);
        }

        return "not done";
    }

    private int DropRock(Stack<string> tower, Queue<List<string>> rockQueue, string gasJets, int jetIndex, int totalJets)
    {
        var rock = rockQueue.Dequeue();
        rockQueue.Enqueue(rock);

        AddNewRockBuffer(tower);
        for (int i = rock.Count-1; i <= 0; i--)
        {
            tower.Push(rock[i]);
        }

        return jetIndex;
    }

    public override dynamic Part2()
    {
        throw new System.NotImplementedException();
    }

    private void PushRock(List<string> rock, string topOfTower, bool left)
    {
        if (left)
        {
            if (rock.Any(r => r[0] == '#'))
                return;
            for (int i = 0; i < topOfTower.Length - 1; i++)
                if (topOfTower[i] == '#' && rock.Last()[i + 1] == '#')
                    return;
        }
        else
        {
            if (rock.Any(r => r[^1] == '#'))
                return;
            
            for (int i = topOfTower.Length - 1; i > 0; i--)
                if (topOfTower[i] == '#' && rock.Last()[i - 1] == '#')
                    return;
        }

        for (int i = 0; i < rock.Count; i++)
        {
            if (left)
                rock[i] = rock[i].Substring(1) + " ";
            else
                rock[i] = " " + rock[i].Substring(0, rock[i].Length - 1);
        }
    }

    private bool WillCollide(List<string> rock, string towerTop)
    {
        for (int r = rock.Count - 1; r <= 0; r--)
        {
            var rockSlice = rock[r];
            for (int i = 0; i < towerTop.Length; i++)
            {
                if (rockSlice[i] == '#' && rockSlice[i] == towerTop[i])
                    return true;
            }   
        }

        return false;
    }

    private Queue<List<string>> getInitialRockQueue()
    {
        var queue = new Queue<List<string>>();
        queue.Enqueue(_flat);
        queue.Enqueue(_plus);
        queue.Enqueue(_backwardL);
        queue.Enqueue(_tall);
        queue.Enqueue(_square);
        return queue;
    }

    private void AddNewRockBuffer(Stack<string> tower)
    {
        tower.Push("       ");
        tower.Push("       ");
        tower.Push("       ");
    }

    private List<string> _flat = new List<string>
    {
        "  #### ",
    };
    
    private List<string> _plus = new List<string>
    {
        "   #   ",
        "  ###  ",
        "   #   "
    };
    
    private List<string> _backwardL = new List<string>
    {
        "    #  ",
        "    #  ",
        "   ##  "
    };
    
    private List<string> _tall = new List<string>
    {
        "  #    ",
        "  #    ",
        "  #    ",
        "  #    "
    };
    
    private List<string> _square = new List<string>
    {
        "  ##   ",
        "  ##   "
    };
}