using System.Collections.Generic;

namespace AOC2022.Modules;

public class Day17 : DayBase
{
    public override bool Ignore { get; }
    public override dynamic Part1()
    {
        throw new System.NotImplementedException();
    }

    public override dynamic Part2()
    {
        throw new System.NotImplementedException();
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

    private List<string> _newRockBuffer = new List<string>
    {
        "       ",
        "       ",
        "       ",
    };

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